using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class StickerInfo : ChosenInlineResultHandler
    {
        protected override string CommandText => Command.sticker_info;
        
        public override async Task Execute()
        {
            var hash = InlineResult.Split('=')[1];
            var sticker = await StickerDao.GetByHash(hash);
            var stickerId = User.Session.State is UserState.AuctionMenu or UserState.ShopMenu
                ? sticker.IdWithWatermark
                : sticker.Id;
            await MessageController.SendSticker(User, stickerId);
            await MessageController.EditMessage(User, sticker.ToString(), Keyboard.StickerInfoKeyboard);
        }

        public StickerInfo(UserEntity user, Update update) : base(user, update) { }
    }
}