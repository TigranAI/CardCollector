using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Auction
{
    [SkipCommand]
    public class ShowTraders : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AuctionModule>();
            var tradersList = await Context.Auctions.FindAllOrders(module.SelectedStickerId, InlineQuery.Query);
            User.Session.GetModule<FiltersModule>().ApplyPriceTo(tradersList);
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > tradersList.Count ? "" : (offset + 50).ToString();
            var discount = User.HasAuctionDiscount() ? 0.95 : 1;
            var results = tradersList
                .Skip(offset)
                .Take(50)
                .Select(item => item.AsTelegramArticle(discount));
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (User.Session.State is not UserState.ProductMenu) return false;
            if (InlineQuery.ChatType is not ChatType.Sender) return false;
            return User.Session.GetModule<AuctionModule>().SelectedStickerId is not null;
        }
    }
}