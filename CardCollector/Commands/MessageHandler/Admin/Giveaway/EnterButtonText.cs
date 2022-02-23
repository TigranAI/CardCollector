using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MessageHandler.Admin.Giveaway
{
    public class EnterButtonText : MessageHandler
    {
        protected override string CommandText => "";

        private static readonly LinkedList<long> Queue = new();

        protected override async Task Execute()
        {
            RemoveFromQueue(User.Id);
            var module = User.Session.GetModule<AdminModule>();
            if (module.SelectedChannelGiveawayId == null) return;
            var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId.Value);
            giveaway.ButtonText = Message.Text;
            await User.Messages.EditMessage(User, Messages.send_giveaway_image,
                Keyboard.SkipKeyboard(typeof(SendGiveawayImage).Name));
            SendGiveawayImage.AddToQueue(User.Id);
        }

        public static async Task Skip(User user, BotDatabaseContext context)
        {
            RemoveFromQueue(user.Id);
            var module = user.Session.GetModule<AdminModule>();
            if (module.SelectedChannelGiveawayId == null) return;
            var giveaway = await context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId.Value);
            giveaway.ButtonText = Text.claim_prize;
            await user.Messages.EditMessage(user, Messages.send_giveaway_image,
                Keyboard.SkipKeyboard(typeof(SendGiveawayImage).Name));
            SendGiveawayImage.AddToQueue(user.Id);
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
            return Queue.Contains(User.Id) && Message.Type == MessageType.Text;
        }

        public EnterButtonText(User user, BotDatabaseContext context, Message message) : base(user, context,
            message)
        {
        }
    }
}