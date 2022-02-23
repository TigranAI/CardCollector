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
    public class EnterPrizeCount : MessageHandler
    {
        protected override string CommandText => "";

        private static readonly LinkedList<long> Queue = new();

        protected override async Task Execute()
        {
            if (int.TryParse(Message.Text, out var count))
            {
                RemoveFromQueue(User.Id);
                var module = User.Session.GetModule<AdminModule>();
                if (module.SelectedChannelGiveawayId == null) return;
                var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId.Value);
                giveaway.PrizeCount = count;
                await User.Messages.EditMessage(User, Messages.enter_giveaway_message, Keyboard.BackKeyboard,
                    ParseMode.Html);
                EnterGiveawayMessage.AddToQueue(User.Id);
            }
            else await User.Messages.EditMessage(User, Messages.please_enter_integer, Keyboard.BackKeyboard);
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

        public EnterPrizeCount(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}