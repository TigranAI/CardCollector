using System.Threading.Tasks;
using CardCollector.Commands.Message;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyCoins : CallbackQueryCommand
    {
        protected override string CommandText => Command.buy_coins;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            EnterGemsExchange.AddToQueue(User.Id);
            var module = User.Session.GetModule<ShopModule>();
            await MessageController.EditMessage(User,
                $"{Messages.exchange_mesage}" +
                $"\n{Messages.gems_exchange_count} {module.EnteredExchangeSum}{Text.gem}" +
                $"\n{Messages.coins_exchange_count} {module.EnteredExchangeSum * 150}{Text.coin}" +
                $"\n\n{Messages.enter_exchange_sum}",
                Keyboard.BuyCoinsKeyboard);
        }

        public BuyCoins() { }
        public BuyCoins(UserEntity user, Update update) : base(user, update) { }
    }
}