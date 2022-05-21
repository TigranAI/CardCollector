using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CardCollector.Games;
using CardCollector.Others;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Database.Entity
{
    public class PuzzlePiece : ITelegramInlineQueryResult
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual Sticker Sticker { get; set; }
        [MaxLength(127)] public string FileId { get; set; }
        public int Order { get; set; }
        public int PieceCount { get; set; }
        
        public InlineQueryResult ToResult(string command)
        {
            return new InlineQueryResultCachedSticker(
                $"{command}={Id}", FileId)
            {
                ReplyMarkup = Puzzle.ChooseStickerKeyboard()
            };
        }
    }
}