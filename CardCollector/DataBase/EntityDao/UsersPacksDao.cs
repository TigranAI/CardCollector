using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class UsersPacksDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(UsersPacksDao));
        private static readonly DbSet<UsersPacksEntity> Table = Instance.UsersPacks;

        public static async Task<List<UsersPacksEntity>> GetUserPacks(long userId)
        {
            return (await Table.WhereAsync(item => Task.FromResult(item.UserId == userId))).ToList();
        }

        public static async Task<UsersPacksEntity> GetPackInfo(long userId, int packId)
        {
            return await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.PackId == packId)
                   ?? await AddNew(userId, packId);
        }

        public static async Task<UsersPacksEntity> AddNew(long userId, int packId, int count = 0)
        {
            if (await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.PackId == packId) is { } obj)
                return obj;
            var newPack = new UsersPacksEntity()
            {
                PackId = packId,
                UserId = userId,
                Count = count
            };
            var result = await Table.AddAsync(newPack);
            await Instance.SaveChangesAsync();
            return result.Entity;
        }
    }
}