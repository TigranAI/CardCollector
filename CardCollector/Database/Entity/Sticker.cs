using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

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
        
        public override string ToString()
        {
            var str = $"\n{Title} {TierAsStars()}" +
                      $"\n{Text.emoji}: {Emoji}" +
                      $"\n{Text.author}: {Author}";
            str += Tier != 10
                ? $"\n{Income}{Text.coin} {IncomeTime}{Text.time}{Text.minutes}"
                : $"\n1{Text.candy} 1{Text.sun}{Text.day}";
            if (Effect != Effect.None)
                str += $"\n{Text.effect}: " +
                       $"{EffectTranslations.ResourceManager.GetString(((int) Effect).ToString())}";
            if (ExclusiveTask != ExclusiveTask.None)
            {
                var task = ExclusiveTaskTranslations.ResourceManager.GetString(((int) ExclusiveTask).ToString());
                str += $"\n{Text.upgradable}: " +
                       $"{string.Format(task, ExclusiveTaskGoal)}";
            }
            if (Description != "") str += $"\n\n{Text.description}: {Description}";
            return str;
        }

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

        public string ToString(int count)
        {
            var str = $"\n{Title} {TierAsStars()}" +
                      $"\n{Text.emoji}: {Emoji}" +
                      $"\n{Text.author}: {Author}" +
                      $"\n{Text.count}: {count}";
            str += Tier != 10
                ? $"\n{Income}{Text.coin} {IncomeTime}{Text.time}{Text.minutes}"
                : $"\n1{Text.candy} 1{Text.sun}{Text.day}";
            if (Effect != Effect.None)
                str += $"\n{Text.effect}: " +
                       $"{EffectTranslations.ResourceManager.GetString(((int) Effect).ToString())}";
            if (ExclusiveTask != ExclusiveTask.None)
            {
                var task = ExclusiveTaskTranslations.ResourceManager.GetString(((int) ExclusiveTask).ToString());
                str += $"\n{Text.upgradable}: " +
                       $"{string.Format(task, ExclusiveTaskGoal)}";
            }
            if (Description != "") str += $"\n\n{Text.description}: {Description}";
            return str;
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