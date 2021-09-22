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
            var buyPositionCount = User.Session.SelectedSticker.count;
            var stickerCount = User.Session.State switch
            {
                UserState.CollectionMenu => User.Stickers[User.Session.SelectedSticker.Md5Hash].Count,
                UserState.ShopMenu => await ShopController.GetStickerCount(
                    User.Session.SelectedSticker.Id),
                UserState.AuctionMenu => await AuctionController.GetStickerCount(
                    User.Session.SelectedSticker.Id),
                _ => 0
            };
            if (CallbackData.Contains('+'))
            {
                if (buyPositionCount < stickerCount || stickerCount == -1)
                {
                    User.Session.SelectedSticker.count++;
                    await MessageController.EditReplyMarkup(User, CallbackMessageId,
                        Keyboard.GetStickerKeyboard(User.Session.SelectedSticker, User.Session.State));
                }
                else await MessageController.AnswerCallbackQuery(User, 
                    Update.CallbackQuery!.Id, Messages.cant_increase);
            }
            else if (CallbackData.Contains('-'))
            {
                if (buyPositionCount > 1)
                {
                    User.Session.SelectedSticker.count--;
                    await MessageController.EditReplyMarkup(User, CallbackMessageId,
                        Keyboard.GetStickerKeyboard(User.Session.SelectedSticker, User.Session.State));
                }
                else await MessageController.AnswerCallbackQuery(User, 
                    Update.CallbackQuery!.Id, Messages.cant_decrease);
            }
        }

        public CountQuery() { }
        public CountQuery(UserEntity user, Update update) : base(user, update) { }
    }
}