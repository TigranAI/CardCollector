using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ConfirmBuyingQuery : CallbackQuery
    {
        protected override string CommandText => Command.confirm_buying;
        public override async Task Execute()
        {
            var coinsPrice = User.Session.SelectedSticker.GetCoinsPrice();
            var gemsPrice = User.Session.SelectedSticker.GetGemsPrice();
            if (coinsPrice > User.Cash.Coins)
                await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id, Messages.not_enougth_coins);
            else if (gemsPrice > User.Cash.Gems)
                await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id, Messages.not_enougth_gems);
            else
            {
                var text = $"{Messages.confirm_buying}\n{User.Session.SelectedSticker.Count}{Text.items}" +
                           $" {Text.per} {coinsPrice}{Text.coin} / {gemsPrice}{Text.gem}" +
                           $"\n{Text.total}: {coinsPrice*User.Session.SelectedSticker.Count}{Text.coin} / {gemsPrice*User.Session.SelectedSticker.Count}{Text.gem}" +
                           $"\n{Messages.are_you_sure}";
                await MessageController.EditMessage(User, CallbackMessageId, text, Keyboard.GetConfirmationKeyboard(Command.buy_sticker));
            }
        }

        public ConfirmBuyingQuery() { }
        public ConfirmBuyingQuery(UserEntity user, Update update) : base(user, update) { }
    }
}