using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Auction
{
    public class ReturnFromAuction : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.return_from_auction;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AuctionModule>();
            var auction = await Context.Auctions.FindById(module.SelectedAuctionId);
            if (auction == null) return;
            Context.Attach(auction);
            Context.Remove(auction);
            await User.AddSticker(auction.Sticker, auction.Count);
            await User.Messages.EditMessage(
                string.Format(Messages.successfully_returned, auction.Count, auction.Sticker.Title),
                Keyboard.BackKeyboard);
        }
    }
}