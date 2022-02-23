using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.InlineQueryHandler.Auction
{
    [DontAddToCommandStack]
    public class ShowTraders : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AuctionModule>();
            var tradersList = await Context.Auctions.FindAllOrders(module.SelectedStickerId, InlineQuery.Query);
            User.Session.GetModule<FiltersModule>().ApplyPriceTo(tradersList);
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > tradersList.Count ? "" : (offset + 50).ToString();
            var hasDiscount = User.HasAuctionDiscount();
            var results = tradersList.ToTelegramResults(ChosenInlineResultCommands.select_trader,
                offset, hasDiscount ? 0.95 : 1);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (User.Session.State is not UserState.ProductMenu) return false;
            if (InlineQuery.ChatType is not ChatType.Sender) return false;
            return User.Session.GetModule<AuctionModule>().SelectedStickerId is not null;
        }

        public ShowTraders(User user, BotDatabaseContext context, InlineQuery inlineQuery) : base(user,
            context, inlineQuery)
        {
        }
    }
}