using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class UsersPacksDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(UsersPacksDao));
        private static readonly DbSet<UsersPacksEntity> Table = Instance.UsersPacks;

        public static async Task<UsersPacksEntity> GetUserPacks(long userId)
        {
            return await Table.FirstOrDefaultAsync(item => item.UserId == userId) ?? await AddNew(userId);
        }

        public static async Task<UsersPacksEntity> AddNew(long userId)
        {
            var newPack = new UsersPacksEntity(){UserId = userId};
            var result = await Table.AddAsync(newPack);
            return result.Entity;
        }
    }
}