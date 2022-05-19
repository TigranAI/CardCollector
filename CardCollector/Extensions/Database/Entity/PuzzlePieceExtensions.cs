using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Database.Entity;
using CardCollector.Games;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Extensions.Database.Entity;

public static class PuzzlePieceExtensions
{
    public static InlineQueryResultCachedSticker AsTelegramSticker(this PuzzlePiece piece)
    {
        return new InlineQueryResultCachedSticker(
            $"{ChosenInlineResultCommands.select_puzzle}={piece.Id}", piece.FileId)
        {
            ReplyMarkup = Puzzle.ChooseStickerKeyboard()
        };
    }
}