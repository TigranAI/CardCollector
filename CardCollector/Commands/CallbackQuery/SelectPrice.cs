using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectPrice : CallbackQueryCommand
    {
        protected override string CommandText => Command.price;
        public override async Task Execute()
        {
            await MessageController.EditMessage(User, CallbackMessageId, Messages.choose_price, 
                User.Session.State == UserState.AuctionMenu ? Keyboard.GemsPriceOptions : Keyboard.CoinsPriceOptions);
        }

        public SelectPrice() { }
        public SelectPrice(UserEntity user, Update update) : base(user, update) { }
    }
}