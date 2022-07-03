using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Resources.Translations.Providers;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Database.Entity
{
    public class Sticker : ITelegramInlineQueryResult, ITelegramInlineQueryMessageResult
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(256)] public string Title { get; set; }
        [MaxLength(128)] public string Author { get; set; }
        public int Income { get; set; }
        public int IncomeTime { get; set; }
        public IncomeType IncomeType { get; set; } = IncomeType.Coins;
        public int Tier { get; set; }
        public Effect Effect { get; set; }
        public ExclusiveTask ExclusiveTask { get; set; }
        [MaxLength(127)] public string Emoji { get; set; }
        [MaxLength(1024)] public string? Description { get; set; }
        public virtual Pack Pack { get; set; }
        [MaxLength(127)] public string FileId { get; set; }
        public bool IsAnimated { get; set; }
        [MaxLength(127)] public string? ForSaleFileId { get; set; }
        public bool? IsForSaleAnimated { get; set; }
        [MaxLength(127)] public string? GrayFileId { get; set; }
        public int ExclusiveTaskGoal { get; set; }
        public virtual ICollection<PuzzlePiece> PuzzlePieces { get; set; }

        [NotMapped] public string Filename { get; set; }

        public InlineQueryResult ToMessageResult(string command)
        {
            return new InlineQueryResultCachedSticker($"{command}={Id}", FileId)
            {
                InputMessageContent = new InputTextMessageContent(Text.select)
            };
        }

        public InlineQueryResult ToResult(string command)
        {
            return new InlineQueryResultCachedSticker($"{command}={Id}", FileId);
        }

        public override string ToString()
        {
            return ToString(null);
        }

        public string ToString(int? count)
        {
            var builder = new StringBuilder();
            builder.Append($"\n{Title} {TierAsStars()}")
                .Append($"\n{Text.emoji}: {Emoji}")
                .Append($"\n{Text.author}: {Author}");
                
            if (count != null)
                builder.Append($"\n{Text.count}: {count}");
            
            if (Tier != 10)
                builder.Append($"\n{Text.income}: {Income}{Text.coin} {IncomeTime}{Text.time}{Text.minutes}");
            else
                builder.Append($"\n{Text.upgradable_income}: 1{Text.candy} 1{Text.sun}{Text.day}");
            
            if (Effect != Effect.None)
                builder.Append($"\n{Text.effect}: {EffectTranslationsProvider.Instance[Effect]}");
            if (ExclusiveTask != ExclusiveTask.None)
                builder.Append($"\n{Text.upgradable}: {string.Format(ExclusiveTaskTranslationsProvider.Instance[ExclusiveTask]!, ExclusiveTaskGoal)}");

            if (Description != "")
                builder.Append($"\n\n{Text.description}: {Description}");

            return builder.ToString();
        }

        public string TierAsStars()
        {
            if (Tier == 10) return Text.exclusive_star;
            return string.Concat(Enumerable.Repeat(Text.star, Tier));
        }

        public bool Contains(string value)
        {
            if (value == "") return true;
            if (Title.Contains(value, StringComparison.CurrentCultureIgnoreCase)) return true;
            if (Emoji.Contains(value)) return true;
            if (Author.Contains(value, StringComparison.CurrentCultureIgnoreCase)) return true;
            return false;
        }

        public async Task ApplyEffect(User user, UserSticker userSticker)
        {
            switch (Effect)
            {
                case Effect.PiggyBank200:
                    user.Cash.MaxCapacity += 200;
                    await user.Messages.SendMessage(Messages.effect_PiggyBank200);
                    break;
                case Effect.Diamonds25Percent:
                    await user.AddGems((int) (user.Cash.Gems * 0.25));
                    await user.Messages.SendMessage(Messages.effect_Diamonds25Percent);
                    break;
                case Effect.Random1Pack5Day:
                    userSticker.GivePrizeDate = DateTime.Today;
                    break;
                case Effect.RandomSticker1Tier2Day:
                    userSticker.GivePrizeDate = DateTime.Today;
                    break;
                case Effect.RandomSticker2Tier3Day:
                    userSticker.GivePrizeDate = DateTime.Today;
                    break;
            }
        }

        public void SetSticker(Telegram.Bot.Types.Sticker sticker)
        {
            FileId = sticker.FileId;
            IsAnimated = sticker.IsAnimated;
        }

        public void SetWatermark(Telegram.Bot.Types.Sticker sticker)
        {
            ForSaleFileId = sticker.FileId;
            IsForSaleAnimated = sticker.IsAnimated;
        }

        public void SetMonochrome(Telegram.Bot.Types.Sticker sticker)
        {
            GrayFileId = sticker.FileId;
        }

        public void AddPuzzlePieces(List<Telegram.Bot.Types.Sticker> stickers)
        {
            foreach (var sticker in stickers.WithIndex())
            {
                var puzzle = new PuzzlePiece()
                {
                    FileId = sticker.item.FileId,
                    Order = sticker.index,
                    PieceCount = stickers.Count,
                    Sticker = this
                };
                PuzzlePieces.Add(puzzle);
            }
        }
    }
}