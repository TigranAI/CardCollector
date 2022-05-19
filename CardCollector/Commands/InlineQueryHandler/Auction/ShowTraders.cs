using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
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
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var length = 0;
            var module = User.Session.GetModule<AuctionModule>();
            var tradersList = await Context.Auctions.FindAllOrders(module.SelectedStickerId, InlineQuery.Query);
            User.Session.GetModule<FiltersModule>().ApplyPriceTo(tradersList);
            var discount = User.HasAuctionDiscount() ? 0.95 : 1;
            var results = tradersList
                .And(list => length = list.Count)
                .Skip(offset)
                .Take(50)
                .Select(item => item.AsTelegramArticle(discount));
            
            var newOffset = offset + 50 > length ? "" : (offset + 50).ToString();
            await AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (User.Session.State is not UserState.ProductMenu) return false;
            if (InlineQuery.ChatType is not ChatType.Sender) return false;
            return User.Session.GetModule<AuctionModule>().SelectedStickerId is not null;
        }
    }
}