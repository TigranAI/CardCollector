using System.Threading.Tasks;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Group;

public class PuzzleSupergame : CallbackQueryHandler
{
    protected override string CommandText => CallbackQueryCommands.puzzle_supergame;
    protected override async Task Execute()
    {
        await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.puzzle_supergame_info, true);
    }
}