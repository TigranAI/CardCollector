using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector.Commands.PreCheckoutQuery
{
    public class BuyGems : PreCheckoutQueryCommand
    {
        protected override string CommandText => Command.buy_gems_item;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQueryId);
            User.Cash.Gems += 50 * Amount / 100;
            await User.ClearChat();
            var message = await MessageController.SendMessage(User, Messages.thanks_for_buying);
            User.Session.Messages.Add(message.MessageId);
        }

        public BuyGems() { }
        public BuyGems(UserEntity user, Update update) : base(user, update) { }
    }
}