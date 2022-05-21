using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    public class SelectPrice : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.select_price;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(Messages.choose_price, 
                User.Session.State == UserState.AuctionMenu ? Keyboard.GemsPriceOptions : Keyboard.CoinsPriceOptions);
        }
    }
}