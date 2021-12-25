using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    public class Login : MessageCommand
    {
        protected override string CommandText => "login";

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-_";

        public override async Task Execute()
        {
            var data = Update.Message!.Text!.Split(' ')[1].Split('=')[1];
            await MessageController.EditMessage(User, $"{Messages.confirm_login} {AppSettings.SITE_URL}",
                Keyboard.ConfirmLogin(data));
        }

        private string GenerateNewToken()
        {
            return new string(Enumerable.Repeat(chars, 64).Select(s => s[Utilities.rnd.Next(s.Length)]).ToArray());
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            if (update.Message!.Type != MessageType.Text) return false;
            var data = update.Message.Text!.Split(' ');
            if (data.Length < 2) return false;
            var command = data[1].Split('=')[0];
            return command == CommandText;
        }

        public Login()
        {
        }

        public Login(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}