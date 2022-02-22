using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    public class SelectPrice : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.select_price;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.choose_price, 
                User.Session.State == UserState.AuctionMenu ? Keyboard.GemsPriceOptions : Keyboard.CoinsPriceOptions);
        }

        public SelectPrice(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}