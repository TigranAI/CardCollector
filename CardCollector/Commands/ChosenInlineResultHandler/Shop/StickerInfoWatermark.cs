using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;

namespace CardCollector.Commands.ChosenInlineResultHandler.Shop
{
    public class StickerInfoWatermark : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.sticker_info;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            var sticker = await Context.Stickers.FindById(stickerId);
            await User.Messages.ClearChat();
            await User.Messages.SendSticker(sticker.ForSaleFileId ?? sticker.FileId);
            await User.Messages.SendMessage(sticker.ToString(), Keyboard.StickerInfoKeyboard);
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State is UserState.ShopMenu or UserState.AuctionMenu;
        }
    }
}