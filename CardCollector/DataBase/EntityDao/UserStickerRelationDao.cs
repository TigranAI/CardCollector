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
        private static async Task<UserStickerRelationEntity> AddNew(long userId, string stickerId, int count)
        {
            var cash = new UserStickerRelationEntity
            {
                UserId = userId,
                StickerId = stickerId,
                Count = count,
                ShortHash = Utilities.CreateMD5(stickerId + userId)
            };
            var result = await Table.AddAsync(cash);
            return result.Entity;
        }
    }
}