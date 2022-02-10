using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.TextCommands
{
    public class Login : MessageHandler
    {
        protected override string CommandText => MessageCommands.login;

        protected override async Task Execute()
        {
            var data = Message.Text!.Split(' ')[1].Split('=')[1];
            await User.Messages.EditMessage(User, $"{Messages.confirm_login} {AppSettings.SITE_URL}",
                Keyboard.ConfirmLogin(data));
        }

        public override bool Match()
        {
            if (Message.Type != MessageType.Text) return false;
            var data = Message.Text!.Split(' ');
            if (data.Length < 2) return false;
            var command = data[1].Split('=')[0];
            return command == CommandText;
        }

        public Login(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}