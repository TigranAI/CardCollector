using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

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
            await User.Messages.EditMessage(User,
                string.Format(Messages.successfully_returned, auction.Count, auction.Sticker.Title),
                Keyboard.BackKeyboard);
        }

        public ReturnFromAuction(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}