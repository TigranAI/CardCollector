using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class Count : CallbackQueryCommand
    {
        protected override string CommandText => Command.count;
        public override async Task Execute()
        {
            var (stickerCount, maxCount) = User.Session.State switch
            {
                UserState.CollectionMenu when User.Session.GetModule<CollectionModule>() is {} module => 
                    (module.Count, User.Stickers[module.SelectedSticker.Md5Hash].Count),
                UserState.ProductMenu when User.Session.GetModule<AuctionModule>() is {} module => 
                    (module.Count, module.MaxCount),
                UserState.CombineMenu when User.Session.GetModule<CombineModule>() is {} module => 
                    (module.Count, User.Stickers[module.SelectedSticker.Md5Hash].Count),
                _ => (0, 0)
            };
            var changed = false;
            if (CallbackData.Contains(Text.plus) && (stickerCount < maxCount || maxCount == -1))
            {
                stickerCount++;
                changed = true;
            }
            else if (CallbackData.Contains(Text.minus) && stickerCount > 1)
            {
                stickerCount--;
                changed = true;
            }
            switch(User.Session.State)
            {
                case UserState.CollectionMenu:
                    User.Session.GetModule<CollectionModule>().Count = stickerCount;
                    break;
                case UserState.ProductMenu: 
                    User.Session.GetModule<AuctionModule>().Count = stickerCount;
                    break;
                case UserState.CombineMenu: 
                    User.Session.GetModule<CombineModule>().Count = stickerCount;
                    break;
            }
            if (changed) await MessageController.EditReplyMarkup(User, CallbackMessageId, Keyboard.GetStickerKeyboard(User.Session));
            else await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id, Messages.cant_change_count);
        }

        public Count() { }
        public Count(UserEntity user, Update update) : base(user, update) { }
    }
}