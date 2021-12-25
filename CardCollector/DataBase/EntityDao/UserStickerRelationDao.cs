using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    /* Предоставляет доступ к соотношениям таблицы user_to_sticker_relation */
    public static class UserStickerRelationDao
    {
        public static BotDatabase Instance;
        public static DbSet<UserStickerRelation> Table;

        static UserStickerRelationDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(UserStickerRelationDao));
            Table = Instance.UserStickerRelations;
        }

        /* Возвращает словарь стикеров по Id пользователя */
        public static async Task<Dictionary<string, UserStickerRelation>> GetListById(long userId)
        {
            try
            {
                return await Table.Where(i => i.UserId == userId).ToDictionaryAsync(p => p.ShortHash, p => p);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetListById(userId);
            }
        }

        /* Добавляет новое отношение в таблицу */
        public static async Task<UserStickerRelation> AddSticker(UserEntity user, StickerEntity sticker, int count = 1)
        {
            try
            {
                if (user.Stickers.ContainsKey(sticker.Md5Hash))
                {
                    user.Stickers[sticker.Md5Hash].Count += count;
                    return user.Stickers[sticker.Md5Hash];
                }

                var relation = new UserStickerRelation
                {
                    UserId = user.Id,
                    StickerId = sticker.Id,
                    Count = count,
                    ShortHash = sticker.Md5Hash
                };
                await sticker.ApplyEffect(user, relation);
                var result = await Table.AddAsync(relation);
                user.Stickers.Add(sticker.Md5Hash, result.Entity);
                await Instance.SaveChangesAsync();
                return result.Entity;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await AddSticker(user, sticker, count);
            }
        }

        public static async Task<List<UserStickerRelation>> GetListWhere(
            Expression<Func<UserStickerRelation, bool>> predicate)
        {
            try
            {
                return await Table.Where(predicate).ToListAsync();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetListWhere(predicate);
            }
        }
    }
}