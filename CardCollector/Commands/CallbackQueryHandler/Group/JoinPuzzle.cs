using System.Threading.Tasks;
using CardCollector.Cache.Repository;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Games;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Group;

public class JoinPuzzle : CallbackQueryHandler
{
    protected override string CommandText => CallbackQueryCommands.join_puzzle;

    protected override async Task Execute()
    {
        var chatId = long.Parse(CallbackQuery.Data!.Split("=")[1]);
        var userRepo = new UserInfoRepository();
        var userInfo = await userRepo.GetAsync(User);

        if (userInfo.PuzzleChatId != 0)
        {
            await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.you_are_now_in_game);
            return;
        }

        var chat = await Context.TelegramChats.FindById(chatId);

        var puzzleRepo = new PuzzleInfoRepository();
        var puzzleInfo = await puzzleRepo.GetAsync(chat!);

        if (puzzleInfo.StickerId != -1)
        {
            await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.puzzle_now_started);
            return;
        }

        if (puzzleInfo.CreatorId == 0)
        {
            await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.puzzle_now_ended);
            return;
        }

        if (puzzleInfo.Players.Count < 4)
        {
            userInfo.PuzzleChatId = chatId;
            puzzleInfo.Players.Add(User.Id);
            if (puzzleInfo.Players.Count < 4) await Puzzle.SendPrepareMessage(puzzleInfo, chat, Context);

            await userRepo.SaveAsync(User, userInfo);
            await puzzleRepo.SaveAsync(chat, puzzleInfo);

            if (puzzleInfo.Players.Count + 1 == Puzzle.PUZZLE_MAX_PLAYERS) await Puzzle.Start(Context, chat);
        }
        else
        {
            await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_slots);
        }
    }
}