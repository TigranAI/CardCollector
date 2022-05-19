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
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var length = 0;
            
            var stickersList = await Context.Auctions.FindAll(User.Session.GetModule<FiltersModule>());
            var results = stickersList
                .Where(item => item.Contains(InlineQuery.Query))
                .And(list => length = list.Count())
                .Skip(offset)
                .Take(50)
                .ToTelegramStickersAsMessage(ChosenInlineResultCommands.select_sticker, offset);
            
            var newOffset = offset + 50 > length ? "" : (offset + 50).ToString();
            await AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            return User.Session.State is UserState.AuctionMenu && InlineQuery.ChatType is ChatType.Sender;
        }
    }
}