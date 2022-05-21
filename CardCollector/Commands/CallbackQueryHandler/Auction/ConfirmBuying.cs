using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Auction
{
    public class ConfirmBuying : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.confirm_buying;

        protected override async Task Execute()
        {
            var auctionModule = User.Session.GetModule<AuctionModule>();
            var auction = await Context.Auctions.FindById(auctionModule.SelectedAuctionId);
            if (auction == null) return;
            var price = auction.Price * auctionModule.Count;
            if (User.HasAuctionDiscount()) price = (int) (price * 0.95);
            if (price > User.Cash.Gems)
                await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_gems, true);
            else
            {
                var text = $"{Messages.confirm_buying}" +
                           $"\n{auctionModule.Count}{Text.items} {Text.per} {price}{Text.gem}" +
                           $"\n{Messages.are_you_sure}";
                await User.Messages.EditMessage(text,
                    Keyboard.GetConfirmationKeyboard(CallbackQueryCommands.buy_sticker));
            }
        }
    }
}