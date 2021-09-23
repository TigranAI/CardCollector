using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CountQuery : CallbackQuery
    {
        protected override string CommandText => Command.count;
        public override async Task Execute()
        {
            var buyPositionCount = User.Session.SelectedSticker.Count;
            if (CallbackData.Contains('+'))
            {
                if (buyPositionCount < User.Session.SelectedSticker.MaxCount || User.Session.SelectedSticker.MaxCount == -1)
                {
                    User.Session.SelectedSticker.Count++;
                    await MessageController.EditReplyMarkup(User, CallbackMessageId, Keyboard.GetStickerKeyboard(User.Session));
                }
                else await MessageController.AnswerCallbackQuery(User, 
                    Update.CallbackQuery!.Id, Messages.cant_increase);
            }
            else if (CallbackData.Contains('-'))
            {
                if (buyPositionCount > 1)
                {
                    User.Session.SelectedSticker.Count--;
                    await MessageController.EditReplyMarkup(User, CallbackMessageId, Keyboard.GetStickerKeyboard(User.Session));
                }
                else await MessageController.AnswerCallbackQuery(User, 
                    Update.CallbackQuery!.Id, Messages.cant_decrease);
            }
        }

        public CountQuery() { }
        public CountQuery(UserEntity user, Update update) : base(user, update) { }
    }
}