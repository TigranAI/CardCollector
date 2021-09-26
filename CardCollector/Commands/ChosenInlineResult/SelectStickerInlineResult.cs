using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SelectStickerInlineResult : ChosenInlineResult
    {
        protected override string CommandText => Command.select_sticker;
        public override async Task Execute()
        {
            await User.ClearChat();
            var hash = InlineResult.Split('=')[1];
            var sticker = await StickerDao.GetStickerByHash(hash);
            var stickerCount = User.Session.State switch
            {
                UserState.AuctionMenu => await AuctionController.GetStickerCount(sticker.Id, User.Session.Filters),
                UserState.ShopMenu => await ShopController.GetStickerCount(sticker.Id),
                _ => User.Stickers[sticker.Md5Hash].Count
            };
            var stickerInfo = new StickerInfo(sticker) {MaxCount = stickerCount};
            User.Session.SelectedSticker = stickerInfo;
            var stickerMessage = await MessageController.SendSticker(User, sticker.Id);
            var infoMessage = await MessageController.SendMessage(User, stickerInfo.ToString(), Keyboard.GetStickerKeyboard(User.Session));
            User.Session.Messages.Add(stickerMessage.MessageId);
            User.Session.Messages.Add(infoMessage.MessageId);
        }

        public SelectStickerInlineResult() { }
        public SelectStickerInlineResult(UserEntity user, Update update) : base(user, update) { }
    }
}