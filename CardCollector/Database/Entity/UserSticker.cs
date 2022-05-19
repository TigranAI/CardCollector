using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Database.Entity
{
    public class UserSticker
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

        public InlineQueryResultCachedSticker AsTelegramCachedSticker(string command)
        {
            return new InlineQueryResultCachedSticker($"{command}={Id}", GetFileId());
        }
    }
}