using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler.Auction
{
    public class SelectAuctionSticker : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.select_sticker;

        protected override async Task Execute()
        {
            User.Session.State = UserState.ProductMenu;
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            User.Session.GetModule<AuctionModule>().SelectedStickerId = stickerId;
            var sticker = await Context.Stickers.FindById(stickerId);
            var positionsCount = await Context.Auctions.GetCountByStickerId(stickerId, User.Session.GetModule<FiltersModule>());
            await User.Messages.ClearChat(User);
            await User.Messages.SendSticker(User, sticker.ForSaleFileId ?? sticker.FileId);
            await User.Messages.SendMessage(User, sticker.ToString(positionsCount), Keyboard.AuctionStickerKeyboard);
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State == UserState.AuctionMenu;
        }

        public SelectAuctionSticker(User user, BotDatabaseContext context, ChosenInlineResult chosenInlineResult) :
            base(user, context, chosenInlineResult)
        {
        }
    }
}