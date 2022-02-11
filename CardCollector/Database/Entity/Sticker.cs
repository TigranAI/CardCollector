using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.StickerEffects;

namespace CardCollector.DataBase.Entity
{
    public class Sticker
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(256)] public string Title { get; set; }
        [MaxLength(128)] public string Author { get; set; }
        public int Income { get; set; }
        public int IncomeTime { get; set; }
        public int Tier { get; set; }
        public Effect Effect { get; set; }
        [MaxLength(127)] public string Emoji { get; set; }
        [MaxLength(1024)] public string? Description { get; set; }
        public virtual Pack Pack { get; set; }
        [MaxLength(127)] public string FileId { get; set; }
        public bool IsAnimated { get; set; }
        [MaxLength(127)] public string? ForSaleFileId { get; set; }
        public bool? IsForSaleAnimated { get; set; }

        public override string ToString()
        {
            var str = $"\n{Title} {string.Concat(Enumerable.Repeat(Text.star, Tier))}" +
                             $"\n{Text.emoji}: {Emoji}" +
                             $"\n{Text.author}: {Author}" +
                             $"\n{Income}{Text.coin} {IncomeTime}{Text.time}{Text.minutes}";
            if (Effect != 0) str += $"\n{Text.effect}: {EffectTranslations.ResourceManager.GetString(Effect.ToString())}";
            if (Description != "") str += $"\n\n{Text.description}: {Description}";
            return str;
        }
        
        public string ToString(int count)
        {
            var str = $"\n{Title} {string.Concat(Enumerable.Repeat(Text.star, Tier))}" +
                      $"\n{Text.emoji}: {Emoji}" +
                      $"\n{Text.author}: {Author}" +
                      $"\n{Text.count}: {(count != -1 ? count : "∞")}" +
                      $"\n{Income}{Text.coin} {IncomeTime}{Text.time}{Text.minutes}";
            if (Effect != 0) str += $"\n{Text.effect}: {EffectTranslations.ResourceManager.GetString(Effect.ToString())}";
            if (Description != "") str += $"\n\n{Text.description}: {Description}";
            return str;
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
                    await user.Messages.SendMessage(user, Messages.effect_PiggyBank200);
                    break;
                case Effect.Diamonds25Percent:
                    user.Cash.Gems += (int)(user.Cash.Gems * 0.25);
                    await user.Messages.SendMessage(user, Messages.effect_Diamonds25Percent);
                    break;
                case Effect.Random1Pack5Day:
                    userSticker.GivePrizeDate = DateTime.Today;
                    break;
                case Effect.RandomSticker1Tier3Day:
                    userSticker.GivePrizeDate = DateTime.Today;
                    break;
                case Effect.RandomSticker2Tier3Day:
                    userSticker.GivePrizeDate = DateTime.Today;
                    break;
            }
        }
    }
}