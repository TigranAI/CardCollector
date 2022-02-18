using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Admin.Giveaway
{
    public class EnterGiveawayMessage : MessageHandler
    {
        protected override string CommandText => "";

        private static readonly LinkedList<long> Queue = new();

        protected override async Task Execute()
        {
            RemoveFromQueue(User.Id);
            var module = User.Session.GetModule<AdminModule>();
            if (module.SelectedChannelGiveawayId == null) return;
            var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId.Value);
            giveaway.Message = Message.Text;
            await User.Messages.EditMessage(User, Messages.enter_when_giveaway_will_be_sended,
                Keyboard.SkipKeyboard(typeof(EnterSendDatetime).Name));
            EnterSendDatetime.AddToQueue(User.Id);
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

        public EnterGiveawayMessage(User user, BotDatabaseContext context, Message message) : base(user, context,
            message)
        {
        }
    }
}