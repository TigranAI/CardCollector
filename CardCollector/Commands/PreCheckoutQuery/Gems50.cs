using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector.Commands.PreCheckoutQuery
{
    public class Gems50 : PreCheckoutQuery
    {
        protected override string CommandText => Command.gems50;
        public override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQueryId);
            User.Cash.Gems += 50 * Amount / 100;
            await User.ClearChat();
            var message = await MessageController.SendMessage(User, Messages.thanks_for_buying);
            User.Session.Messages.Add(message.MessageId);
        }

        public Gems50() { }
        public Gems50(UserEntity user, Update update) : base(user, update) { }
    }
}