using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    /* Предоставляет доступ к соотношениям таблицы user_to_sticker_relation */
    public static class UserStickerRelationDao
    {
        /* Таблица user_to_sticker_relation в представлении Entity Framework */
        private static readonly DbSet<UserStickerRelationEntity> Table = CardCollectorDatabase.Instance.UserStickerRelations;
        
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
            var result = await Table.AddAsync(relation);
            user.Stickers.Add(sticker.Md5Hash, result.Entity);
            return result.Entity;
        }
    }
}