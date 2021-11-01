using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.StickerEffects;

namespace CardCollector.DataBase.Entity
{
    /* Объект таблицы stickers (одна строка)
     Здесь хранится Id стикера с серверов Telegram
     Наименование стикера, Автор стикера, Доход в монетах,
     Доход в алмазах, Количество звезд, Эмоции связанные со стикером,
     Описание стикера
     */
    [Table("stickers")]
    public class StickerEntity
    {
        /* Id стикера на сервере Телеграм */
        [Column("id"), MaxLength(127)] public string Id { get; set; }
        
        /* Название стикера */
        [Column("title"), MaxLength(256)] public string Title { get; set; }
        
        /* Автор стикера */
        [Column("author"), MaxLength(128)] public string Author { get; set; }
        
        /* Доход от стикера в монетах */
        [Column("income"), MaxLength(32)] public int Income { get; set; } = 0;
        
        /* Время, необходимое для получения дохода */
        [Column("income_time"), MaxLength(32)] public int IncomeTime { get; set; } = 1;
        
        /* Количество звезд стикера (редкость) */
        [Column("tier"), MaxLength(32)] public int Tier { get; set; } = 1;
        
        /* Эмоции, связанные со стикером */
        [Column("emoji"), MaxLength(127)] public string Emoji { get; set; } = "";
        
        /* Описание стикера */
        [Column("description"), MaxLength(1024)] public string Description { get; set; } = "";
        
        /* Хеш id стикера для определения его в системе */
        [Column("md5hash"), MaxLength(40)] public string Md5Hash { get; set; }
        
        [Column("effect"), MaxLength(32)] public int Effect { get; set; }
        
        [Column("pack_id"), MaxLength(32)] public int PackId { get; set; }

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
            return value != ""
                ? Title.Contains(value, StringComparison.CurrentCultureIgnoreCase) ||
                  Author.Contains(value, StringComparison.CurrentCultureIgnoreCase) ||
                  Emoji.Equals(value)
                : true;
        }

        public async Task ApplyEffect(UserEntity user, UserStickerRelationEntity relation)
        {
            switch ((Effect)Effect)
            {
                case StickerEffects.Effect.PiggyBank200:
                    user.Cash.MaxCapacity += 200;
                    await MessageController.EditMessage(user, Messages.effect_PiggyBank200);
                    break;
                case StickerEffects.Effect.Diamonds25Percent:
                    user.Cash.Gems += (int)(user.Cash.Gems * 0.25);
                    await MessageController.EditMessage(user, Messages.effect_Diamonds25Percent);
                    break;
                case StickerEffects.Effect.Random1Pack5Day:
                    relation.AdditionalData = DateTime.Today.ToString(CultureInfo.CurrentCulture);
                    break;
                case StickerEffects.Effect.RandomSticker1Tier3Day:
                    relation.AdditionalData = DateTime.Today.ToString(CultureInfo.CurrentCulture);
                    break;
                case StickerEffects.Effect.RandomSticker2Tier3Day:
                    relation.AdditionalData = DateTime.Today.ToString(CultureInfo.CurrentCulture);
                    break;
            }
        }
    }
}