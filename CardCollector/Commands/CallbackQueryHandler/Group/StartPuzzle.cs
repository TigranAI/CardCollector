using System.Threading.Tasks;
using CardCollector.Cache.Repository;
using CardCollector.Database.EntityDao;
using CardCollector.Games;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Group;

public class StartPuzzle : CallbackQueryHandler
{
    protected override string CommandText => CallbackQueryCommands.start_puzzle;
    protected override async Task Execute()
    {
        var userRepo = new UserInfoRepository();
        var userInfo = await userRepo.GetAsync(User);
        if (userInfo.PuzzleChatId == 0)
        {
            await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.you_are_not_in_game);
            return;
        }

        var chat = await Context.TelegramChats.FindById(userInfo.PuzzleChatId);
        var puzzleRepo = new PuzzleInfoRepository();
        var puzzleInfo = await puzzleRepo.GetAsync(chat!);
        
        if (puzzleInfo.CreatorId != User.Id)
        {
            await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.you_are_not_creator);
            return;
        }

        await Puzzle.Start(Context, chat);
    }
}