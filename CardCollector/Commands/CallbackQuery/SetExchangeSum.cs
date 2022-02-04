using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SetExchangeSum : CallbackQueryHandler
    {
        protected override string CommandText => Command.set_exchange_sum;

        public override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            module.EnteredExchangeSum = int.Parse(CallbackData.Split('=')[1]); 
            await MessageController.EditMessage(User,
                $"{Messages.exchange_mesage}" +
                $"\n{Messages.gems_exchange_count} {module.EnteredExchangeSum}{Text.gem}" +
                $"\n{Messages.coins_exchange_count} {module.EnteredExchangeSum * 10}{Text.coin}" +
                $"\n\n{Messages.confirm_exchange}",
                Keyboard.BuyCoinsKeyboard(true));
        }
        
        public SetExchangeSum(UserEntity user, Update update) : base(user, update) { }
    }
}