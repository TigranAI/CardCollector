using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Admin.Distribution
{
    public class SendDistributionSticker : MessageHandler
    {
        protected override string CommandText => "";
        
        private static readonly LinkedList<long> Queue = new();
        
        protected override async Task Execute()
        {
            User.Session.State = UserState.Default;
            RemoveFromQueue(User.Id);
            var module = User.Session.GetModule<AdminModule>();

            var distribution = await Context.ChatDistributions.FindById(module.ChatDistributionId!.Value);
            distribution.StickerFileId = Message.Sticker!.FileId;
            
            await User.Messages.SendMessage(User, Messages.create_distribution_buttons,
                Keyboard.DistributionButtonsKeyboard);
        }

        public static async Task Skip(User user, BotDatabaseContext context)
        {
            user.Session.State = UserState.Default;
            RemoveFromQueue(user.Id);
            await user.Messages.SendMessage(user, Messages.create_distribution_buttons,
                Keyboard.DistributionButtonsKeyboard);
        }

        public static void AddToQueue(long userId)
        {
            if (!Queue.Contains(userId)) Queue.AddLast(userId);
        }

        public static void RemoveFromQueue(long userId)
        {
            Queue.Remove(userId);
        }

        public override bool Match()
        {
            return Queue.Contains(User.Id) && Message.Type == MessageType.Sticker;
        }
    }
}