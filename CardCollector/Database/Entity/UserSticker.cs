using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CardCollector.Database.Entity.NotMapped;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Resources.Translations.Providers;
using Telegram.Bot.Types.Enums;
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

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"\n{Sticker.Title} {Sticker.TierAsStars()}")
                .Append($"\n{Text.emoji}: {Sticker.Emoji}")
                .Append($"\n{Text.author}: {Sticker.Author}")
                .Append($"\n{Text.count}: {Count}");
            
            if (Sticker.Tier != 10)
                builder.Append($"\n{Text.income}: {Sticker.Income}{Text.coin} {Sticker.IncomeTime}{Text.time}{Text.minutes}");
            else if (IsUnlocked)
                builder.Append($"\n{Text.income}: 1{Text.candy} 1{Text.sun}{Text.day}");
            else
                builder.Append($"\n{Text.upgradable_income}: 1{Text.candy} 1{Text.sun}{Text.day}");
            
            if (Sticker.Effect != Effect.None)
                builder.Append($"\n{Text.effect}: {EffectTranslationsProvider.Instance[Sticker.Effect]}");
            if (Sticker.ExclusiveTask != ExclusiveTask.None)
                builder.Append($"\n{Text.upgradable}: {string.Format(ExclusiveTaskTranslationsProvider.Instance[Sticker.ExclusiveTask]!, Sticker.ExclusiveTaskGoal)}");

            if (Sticker.Description != "")
                builder.Append($"\n\n{Text.description}: {Sticker.Description}");

            return builder.ToString();
        }
    }
}