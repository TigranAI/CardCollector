using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Extensions;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Commands.InlineQueryHandler.Admin;

public class ShowGiveawayStickers : InlineQueryHandler
{
    protected override async Task Execute()
    {
        var offset = Offset.Of(InlineQuery);
        var length = 0;

        var stickers = await Context.Stickers.ToListAsync();
        var results = stickers
            .Where(item => item.Contains(InlineQuery.Query))
            .And(list => length = list.Count())
            .ToTelegramResults(ChosenInlineResultCommands.set_giveaway_prize, offset);

        await AnswerInlineQuery(User, InlineQuery.Id, results, offset.GetNext(length));
    }

    public override bool Match()
    {
        return User.Session.State is UserState.SelectGiveawayPrize;
    }
}