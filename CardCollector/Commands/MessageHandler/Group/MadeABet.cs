using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Group
{
    public class MadeABet : MessageHandler
    {

        protected override string CommandText => "";
        protected override Task Execute()
        {
            User.Session.GetModule<GroupModule>().SelectBetChatId = Message.Chat.Id;
            return Task.CompletedTask;
        }

        public override bool Match()
        {
            if (Message.ViaBot is not { } bot) return false;
            if (bot.Username != AppSettings.NAME) return false;
            if (Message.Type != MessageType.Text) return false;
            return Message.Chat.Type is ChatType.Group or ChatType.Supergroup;
        }

        public MadeABet(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}