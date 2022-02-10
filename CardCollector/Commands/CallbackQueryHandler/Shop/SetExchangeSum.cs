using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class SetExchangeSum : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.set_exchange_sum;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            module.EnteredExchangeSum = int.Parse(CallbackQuery.Data!.Split('=')[1]); 
            await User.Messages.EditMessage(User,
                $"{Messages.exchange_mesage}" +
                $"\n{Messages.gems_exchange_count} {module.EnteredExchangeSum}{Text.gem}" +
                $"\n{Messages.coins_exchange_count} {module.EnteredExchangeSum * 10}{Text.coin}" +
                $"\n\n{Messages.confirm_exchange}",
                Keyboard.BuyCoinsKeyboard(true));
        }
        
        public SetExchangeSum(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}