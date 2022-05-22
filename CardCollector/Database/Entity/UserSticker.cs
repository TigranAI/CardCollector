using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Database.Entity
{
    public class UserSticker : ITelegramInlineQueryResult, ITelegramInlineQueryMessageResult
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public virtual User User { get; set; }
        public virtual Sticker Sticker { get; set; }
        public bool IsUnlocked { get; set; }
        public int ExclusiveTaskProgress { get; set; }
        public int Count { get; set; }
        public DateTime Payout { get; set; } = DateTime.Now;
        public DateTime GivePrizeDate { get; set; } = DateTime.Today;
        public DateTime LastUsage { get; set; } = DateTime.Now;

        public string GetFileId()
        {
            if (Sticker.Tier != 10 || IsUnlocked) return Sticker.FileId;
            return Sticker.GrayFileId ?? Sticker.FileId;
        }

        public void UpdateLastUsage()
        {
            LastUsage = DateTime.Now;
        }

        public InlineQueryResult ToResult(string command)
        {
            return new InlineQueryResultCachedSticker($"{command}={Id}", GetFileId());
        }

        public InlineQueryResult ToMessageResult(string command)
        {
            var betMessage =
                new InputTextMessageContent($"{User.Username} {Text.bet} {Sticker.Title} {Sticker.TierAsStars()}");
            return new InlineQueryResultArticle($"{command}={Id}", Sticker.Title, betMessage)
            {
                Description = $"{Text.tier}: {Sticker.TierAsStars()} | {Text.count}: {Count}",
                ReplyMarkup = Keyboard.AnswerBet
            };
        }
    }
}