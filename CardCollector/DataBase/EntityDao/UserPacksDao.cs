using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class UserPacksDao
    {
        public static async Task<List<UserPacks>> GetUserPacks(long userId)
        {
            var Table = BotDatabase.Instance.UsersPacks;
            return await Table.Where(item => item.UserId == userId).ToListAsync();
        }

        public static async Task<UserPacks> AddNew(long userId, int packId)
        {
            var Table = BotDatabase.Instance.UsersPacks;
            var newPack = new UserPacks() { UserId = userId, PackId = packId };
            var result = await Table.AddAsync(newPack);
            await BotDatabase.SaveData();
            return result.Entity;
        }

        public static async Task<UserPacks> GetOne(long userId, int packId)
        {
            var Table = BotDatabase.Instance.UsersPacks;
            return await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.PackId == packId)
                   ?? await AddNew(userId, packId);
        }
    }
}