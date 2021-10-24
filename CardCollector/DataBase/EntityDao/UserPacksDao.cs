using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class UserPacksDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(UserPacksDao));
        private static readonly DbSet<UserPacks> Table = Instance.UsersPacks;

        public static async Task<List<UserPacks>> GetUserPacks(long userId)
        {
            return (await Table.WhereAsync(item => item.UserId == userId)).ToList();
        }

        public static async Task<UserPacks> AddNew(long userId, int packId)
        {
            var newPack = new UserPacks() { UserId = userId, PackId = packId };
            var result = await Table.AddAsync(newPack);
            return result.Entity;
        }

        public static async Task<UserPacks> GetOne(long userId, int packId)
        {
            return await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.PackId == packId)
                   ?? await AddNew(userId, packId);
        }
    }
}