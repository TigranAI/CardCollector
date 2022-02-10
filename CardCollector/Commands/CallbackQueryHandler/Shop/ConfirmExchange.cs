using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class ConfirmExchange : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.confirm_exchange;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            if (module.EnteredExchangeSum > User.Cash.Gems)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_gems, true);
            else
            {
                User.Cash.Gems -= module.EnteredExchangeSum;
                User.Cash.Coins += module.EnteredExchangeSum * 10;
                await User.Messages.EditMessage(User,
                    $"{Messages.you_got} {module.EnteredExchangeSum * 10}{Text.coin} {Text.per} {module.EnteredExchangeSum}{Text.gem}");
            }
        }

        public ConfirmExchange(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}