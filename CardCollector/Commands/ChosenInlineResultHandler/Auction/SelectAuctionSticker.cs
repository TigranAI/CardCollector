using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.ChosenInlineResultHandler.Auction
{
    public class SelectAuctionSticker : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.select_sticker;

        protected override async Task Execute()
        {
            User.Session.State = UserState.ProductMenu;
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            var module = User.Session.GetModule<AuctionModule>();
            module.Count = 1;
            module.SelectedStickerId = stickerId;
            var sticker = await Context.Stickers.FindById(stickerId);
            var positionsCount =
                await Context.Auctions.GetCountByStickerId(stickerId, User.Session.GetModule<FiltersModule>());
            await User.Messages.ClearChat();
            await User.Messages.SendSticker(sticker.ForSaleFileId ?? sticker.FileId);
            await User.Messages.SendMessage(sticker.ToString(positionsCount), Keyboard.AuctionStickerKeyboard);
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State == UserState.AuctionMenu;
        }
    }
}