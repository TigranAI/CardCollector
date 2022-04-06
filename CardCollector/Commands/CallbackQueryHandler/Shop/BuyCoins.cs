using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class BuyCoins : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.buy_coins;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            await User.Messages.EditMessage(User,
                $"{Messages.exchange_mesage}" + 
                $"\n{Messages.gems_exchange_count} {module.EnteredExchangeSum}{Text.gem}" +
                $"\n{Messages.coins_exchange_count} {module.EnteredExchangeSum * 150}{Text.coin}" +
                $"\n\n{Messages.choose_exchange_sum}",
                Keyboard.BuyCoinsKeyboard());
        }
    }
}