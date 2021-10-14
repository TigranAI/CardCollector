using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class SpecificPackDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(SpecificPackDao));
        private static readonly DbSet<SpecificPacksEntity> Table = Instance.SpecificPacks;

        public static async Task<SpecificPacksEntity> GetInfo(long userId, int packId)
        {
            return await Table.FirstOrDefaultAsync(item => item.PackId == packId && item.UserId == userId)
                   ?? await AddNew(userId, packId, 0);
        }

        public static async Task<SpecificPacksEntity> AddNew(long userId, int packId, int count)
        {
            return (await Table.AddAsync(new SpecificPacksEntity
            {
                UserId = userId,
                PackId = packId,
                Count = count
            })).Entity;
        }

        public static async Task<int> GetCount(long userId)
        {
            return await Table.SumAsync(item => Task.FromResult(item.Count));
        }

        public static async Task<List<SpecificPacksEntity>> GetUserPacks(long userId)
        {
            return (await Table.WhereAsync(item => item.UserId == userId && item.Count > 0)).ToList();
        }
    }
}