using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Admin.Giveaway
{
    public class SendGiveawayImage : MessageHandler
    {
        protected override string CommandText => "";

        private static readonly LinkedList<long> Queue = new();

        protected override async Task Execute()
        {
            RemoveFromQueue(User.Id);
            var module = User.Session.GetModule<AdminModule>();
            if (module.SelectedChannelGiveawayId == null) return;
            var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId.Value);
            giveaway.ImageFileId = Message.Photo?.First().FileId;
            await User.Messages.ClearChat(User);
            await User.Messages.SendPhoto(User, giveaway.ImageFileId!, giveaway.GetFormattedMessage(),
                giveaway.GetFormattedKeyboard(CallbackQueryCommands.ignore));
            await User.Messages.SendMessage(User, Messages.please_confirm_this_giveaway, Keyboard.CreateGiveaway);
        }

        public static async Task Skip(User user, BotDatabaseContext context)
        {
            RemoveFromQueue(user.Id);
            var module = user.Session.GetModule<AdminModule>();
            if (module.SelectedChannelGiveawayId == null) return;
            var giveaway = await context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId.Value);
            await user.Messages.ClearChat(user);
            await user.Messages.SendMessage(user, giveaway.GetFormattedMessage(), 
                giveaway.GetFormattedKeyboard(CallbackQueryCommands.ignore));
            await user.Messages.SendMessage(user, Messages.please_confirm_this_giveaway, Keyboard.CreateGiveaway);
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
            return Queue.Contains(User.Id) && Message.Type == MessageType.Photo;
        }

        public SendGiveawayImage(User user, BotDatabaseContext context, Message message) : base(user, context,
            message)
        {
        }
    }
}