using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SelectSticker : ChosenInlineResultCommand
    {
        protected override string CommandText => Command.select_sticker;

        public override async Task Execute()
        {
            var hash = InlineResult.Split('=')[1];
            var sticker = await StickerDao.GetByHash(hash);
            var stickerCount = User.Session.State switch
            {
                UserState.AuctionMenu => await AuctionController.GetStickerCount(sticker.Id, User.Session.GetModule<FiltersModule>()),
                _ => User.Stickers[sticker.Md5Hash].Count
            };
            switch (User.Session.State)
            {
                case UserState.CollectionMenu:
                    User.Session.GetModule<CollectionModule>().SelectedSticker = sticker;
                    User.Session.GetModule<CollectionModule>().Count = 1;
                    break;
                case UserState.AuctionMenu:
                    User.Session.GetModule<AuctionModule>().SelectedSticker = sticker;
                    User.Session.GetModule<AuctionModule>().Count = 1;
                    break;
                case UserState.CombineMenu:
                    User.Session.GetModule<CombineModule>().SelectedSticker = sticker;
                    User.Session.GetModule<CombineModule>().Count = 1;
                    break;
                case UserState.Default:
                    User.Session.GetModule<DefaultModule>().SelectedSticker = sticker;
                    break;
            }
            var stickerId = User.Session.State is UserState.AuctionMenu or UserState.ShopMenu
                ? sticker.IdWithWatermark
                : sticker.Id;
            await MessageController.SendSticker(User, stickerId);
            await MessageController.EditMessage(User, sticker.ToString(stickerCount), Keyboard.GetStickerKeyboard(User.Session));
            if (User.Session.State == UserState.AuctionMenu) User.Session.State = UserState.ProductMenu;
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && 
                   user.Session.State is UserState.CollectionMenu or UserState.AuctionMenu or UserState.CombineMenu or UserState.Default;
        }

        public SelectSticker() { }
        public SelectSticker(UserEntity user, Update update) : base(user, update) { }
    }
}