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
    public class CreateToken : MessageCommand
    {
        protected override string CommandText => "create_token";

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-_";
        private const string site = "http://127.0.0.1:8080/";
        
        public override async Task Execute()
        {
            var token = GenerateNewToken();
            await SessionTokenDao.AddNew(User.Id, token);
            var loginLink = $"{site}login?token={token}";
            await MessageController.EditMessage(User, $"<a href=\"{loginLink}\">{Messages.your_login_link}</a>",
                Keyboard.LoginKeyboard(loginLink), ParseMode.Html);
        }

        private string GenerateNewToken()
        {
            return new string(Enumerable.Repeat(chars, 64).Select(s => s[Utilities.rnd.Next(s.Length)]).ToArray());
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            if (update.Message!.Type != MessageType.Text) return false;
            var data = update.Message.Text!.Split(' ');
            return data.Length > 1 && data[1] == CommandText;
        }

        public CreateToken() { }
        public CreateToken(UserEntity user, Update update) : base(user, update) { }
    }
}