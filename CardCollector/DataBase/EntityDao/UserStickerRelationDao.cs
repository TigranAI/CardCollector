using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class UserStickerRelationDao
    {
        private static readonly DbSet<UserStickerRelationEntity> Table = CardCollectorDatabase.Instance.UserStickerRelations;
        
        public static async Task<Dictionary<string, UserStickerRelationEntity>> GetListById(long userId)
        {
            var result = await Table
                .Where(i => i.UserId == userId)
                .ToDictionaryAsync(p=> p.ShortHash, p=> p);
            return result;
        }

        public static async Task<UserStickerRelationEntity> GetByShortHash(string shortHash)
        {
            return await Table.FirstAsync(i => i.ShortHash == shortHash);
        }

        private static async Task<UserStickerRelationEntity> AddNew(long userId, string stickerId, int count)
        {
            var cash = new UserStickerRelationEntity
            {
                UserId = userId,
                StickerId = stickerId,
                Count = count,
                ShortHash = Utilities.CreateMD5(stickerId)
            };
            var result = await Table.AddAsync(cash);
            return result.Entity;
        }
    }
}