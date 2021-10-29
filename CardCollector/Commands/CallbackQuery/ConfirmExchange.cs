using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ConfirmExchange : CallbackQueryCommand
    {
        protected override string CommandText => Command.confirm_exchange;

        public override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            if (module.EnteredExchangeSum > User.Cash.Gems)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_gems);
            else
            {
                User.Cash.Gems -= module.EnteredExchangeSum;
                User.Cash.Coins += module.EnteredExchangeSum * 150;
                await MessageController.SendMessage(User,
                    $"{Messages.you_got} {module.EnteredExchangeSum * 150}{Text.coin} {Text.per} {module.EnteredExchangeSum}{Text.gem}");
            }
        }

        public ConfirmExchange() { }
        public ConfirmExchange(UserEntity user, Update update) : base(user, update) { }
    }
}