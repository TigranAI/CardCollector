using System.Linq;
using System.Threading.Tasks;
using CardCollector.Cache.Repository;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Extensions.Database.Entity;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Commands.InlineQueryHandler.Group;

public class ShowPuzzlesSticker : InlineQueryHandler
{
    protected override string CommandText => InlineQueryCommands.puzzle;

    protected override async Task<bool> Execute()
    {
        var userRepo = new UserInfoRepository();
        var userInfo = await userRepo.GetAsync(User);
        if (userInfo.PuzzleChatId == 0) return await Ignore();

        var chat = await Context.TelegramChats.FindById(userInfo.PuzzleChatId);
        var puzzleRepo = new PuzzleInfoRepository();
        var puzzleInfo = await puzzleRepo.GetAsync(chat!);
        
        if (puzzleInfo.CreatorId == 0 || puzzleInfo.StickerId == -1) return await Ignore();
        var order = puzzleInfo.GetOrder(User.Id);
        if (order == 0) return await ShowCreatorStickers(puzzleInfo.Players.Count);
        if (order > 0 && puzzleInfo.StickerId != 0) return await ShowPlayerStickers(puzzleInfo.StickerId, order);
        
        return await Ignore();
    }

    private async Task<bool> ShowPlayerStickers(long stickerId, int order)
    {
        var query = InlineQuery.Query.Substring(CommandText.Length).TrimStart();
        
        var puzzlePieces = await Context.PuzzlePieces
            .GetRandomWithSelectedPiece(stickerId, order);
        var results = puzzlePieces
            .Where(item => item.Sticker.Contains(query))
            .Select(item => item.AsTelegramSticker());
        
        await AnswerInlineQuery(User, InlineQuery.Id, results);
        return true;
    }

    private async Task<bool> ShowCreatorStickers(int playersCount)
    {
        var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
        var query = InlineQuery.Query.Substring(CommandText.Length).TrimStart();
        var length = 0;
        
        var puzzlePieces = await Context.PuzzlePieces
            .GetAllByPieceCount(playersCount);
        var results = puzzlePieces
            .Where(item => item.Sticker.Contains(query))
            .And(list => length = list.Count())
            .Skip(offset)
            .Take(50)
            .Select(item => item.AsTelegramSticker());
        
        var newOffset = offset + 50 > length ? "" : (offset + 50).ToString();
        await AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        return true;
    }

    private async Task<bool> Ignore()
    {
        await AnswerInlineQuery(User, InlineQuery.Id, new InlineQueryResultCachedSticker[] { });
        return false;
    }

    public override bool Match()
    {
        if (InlineQuery.ChatType is not (ChatType.Group or ChatType.Supergroup)) return false;
        return InlineQuery.Query.StartsWith(CommandText);
    }
}