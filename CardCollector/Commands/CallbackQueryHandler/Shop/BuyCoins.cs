using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

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

        public BuyCoins(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}