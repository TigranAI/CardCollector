using System.Threading.Tasks;
using CardCollector.Cache.Repository;
using CardCollector.Database.EntityDao;
using CardCollector.Games;

namespace CardCollector.Commands.ChosenInlineResultHandler.Group;

public class SelectPuzzle : ChosenInlineResultHandler
{
    protected override string CommandText => ChosenInlineResultCommands.select_puzzle;
    protected override async Task Execute()
    {
        var userRepo = new UserInfoRepository();
        var userInfo = await userRepo.GetAsync(User);
        
        if (userInfo.PuzzleChatId == 0) return;

        var chat = await Context.TelegramChats.FindById(userInfo.PuzzleChatId);
        var puzzleRepo = new PuzzleInfoRepository();
        var puzzleInfo = await puzzleRepo.GetAsync(chat!);
        
        if (puzzleInfo.CreatorId == 0 || puzzleInfo.StickerId == -1) return;

        var pieceId = long.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
        var piece = await Context.PuzzlePieces.FindById(pieceId);
        
        if (!puzzleInfo.TryMakeAMove(piece)) await Puzzle.BreakGame(Context, chat);
        else if (puzzleInfo.IsEndOfGame()) await Puzzle.EndOfGame(Context, chat, puzzleRepo, puzzleInfo);
        else
        {
            Puzzle.TurnTimers[chat.Id].Reset();
            await puzzleRepo.SaveAsync(chat, puzzleInfo);
        }
    }
}