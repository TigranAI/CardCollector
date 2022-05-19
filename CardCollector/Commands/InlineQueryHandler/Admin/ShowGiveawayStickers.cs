using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Extensions;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Commands.InlineQueryHandler.Admin
{
    public class ShowGiveawayStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var length = 0;
            var stickers = await Context.Stickers.ToListAsync();
            var stickersList = stickers
                .Where(item => item.Contains(InlineQuery.Query))
                .And(list => length = list.Count())
                .Skip(offset)
                .Take(50)
                .ToList();
            var newOffset = offset + 50 > length ? "" : (offset + 50).ToString();
            var results = stickersList
                .ToTelegramStickers(ChosenInlineResultCommands.set_giveaway_prize, offset);
            await AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            return User.Session.State is UserState.SelectGiveawayPrize;
        }
    }
}