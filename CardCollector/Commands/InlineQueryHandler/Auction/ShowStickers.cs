using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Auction
{
    [SkipCommand]
    public class ShowStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var offset = Offset.Of(InlineQuery);
            var length = 0;
            
            var stickersList = await Context.Auctions.FindAll();
            var results = stickersList
                .ApplyFilters(User.Session.GetModule<FiltersModule>())
                .DistinctBy(item => item.Id)
                .Where(item => item.Contains(InlineQuery.Query))
                .And(list => length = list.Count())
                .ToTelegramResults(ChosenInlineResultCommands.select_sticker, offset);
            
            await AnswerInlineQuery(User, InlineQuery.Id, results, offset.GetNext(length));
        }

        public override bool Match()
        {
            return User.Session.State is UserState.AuctionMenu && InlineQuery.ChatType is ChatType.Sender;
        }
    }
}