using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.StickerEffects;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    /* Предоставляет доступ к соотношениям таблицы user_to_sticker_relation */
    public static class UserStickerRelationDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(UserStickerRelationDao));
        /* Таблица user_to_sticker_relation в представлении Entity Framework */
        private static readonly DbSet<UserStickerRelationEntity> Table = Instance.UserStickerRelations;
        
        /* Возвращает словарь стикеров по Id пользователя */
        public static async Task<Dictionary<string, UserStickerRelationEntity>> GetListById(long userId)
        {
            var result = await Table
                .Where(i => i.UserId == userId)
                .ToDictionaryAsync(p=> p.ShortHash, p=> p);
            return result;
        }

        /* Добавляет новое отношение в таблицу */
        public static async Task<UserStickerRelationEntity> AddNew(UserEntity user, StickerEntity sticker, int count)
        {
            if (user.Stickers.ContainsKey(sticker.Md5Hash))
            {
                user.Stickers[sticker.Md5Hash].Count += count;
                return user.Stickers[sticker.Md5Hash];
            }
            var relation = new UserStickerRelationEntity
            {
                UserId = user.Id,
                StickerId = sticker.Id,
                Count = count,
                ShortHash = sticker.Md5Hash
            };
            switch ((Effect)sticker.Effect)
            {
                case Effect.PiggyBank200:
                    user.Cash.MaxCapacity += 200;
                    await MessageController.SendMessage(user, Messages.effect_PiggyBank200);
                    break;
                case Effect.Diamonds25Percent:
                    user.Cash.Gems += (int)(user.Cash.Gems * 0.25);
                    await MessageController.SendMessage(user, Messages.effect_Diamonds25Percent);
                    break;
                case Effect.Random1Pack5Day:
                    relation.AdditionalData = DateTime.Today.ToString(CultureInfo.CurrentCulture);
                    break;
                case Effect.RandomSticker1Tier3Day:
                    relation.AdditionalData = DateTime.Today.ToString(CultureInfo.CurrentCulture);
                    break;
                case Effect.RandomSticker2Tier3Day:
                    relation.AdditionalData = DateTime.Today.ToString(CultureInfo.CurrentCulture);
                    break;
            }
            var result = await Table.AddAsync(relation);
            user.Stickers.Add(sticker.Md5Hash, result.Entity);
            await Instance.SaveChangesAsync();
            return result.Entity;
        }
    }
}