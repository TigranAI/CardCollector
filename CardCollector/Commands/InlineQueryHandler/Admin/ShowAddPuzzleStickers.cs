using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.InlineQueryHandler.Admin;

public class ShowAddPuzzleStickers : InlineQueryHandler
{
    protected override async Task Execute()
    {
        var offset = Offset.Of(InlineQuery);
        var length = 0;
        
        var packId = User.Session.GetModule<AdminModule>().SelectedPackId;
        var pack = await Context.Packs.FindById(packId);
        var results = pack.Stickers
            .Where(sticker => sticker.Contains(InlineQuery.Query))
            .And(list => length = list.Count())
            .ToTelegramResults(ChosenInlineResultCommands.add_puzzle, offset);
        
        await AnswerInlineQuery(User, InlineQuery.Id, results, offset.GetNext(length));
    }

    public override bool Match()
    {
        return User.Session.State is UserState.AddPuzzle;
    }
}