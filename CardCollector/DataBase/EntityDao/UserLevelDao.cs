using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class UserLevelDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(UserLevelDao));
        /* Таблица cash в представлении EntityFramework */
        private static readonly DbSet<UserLevel> Table = Instance.UserLevel;
        
        /* Получение объекта по Id */
        public static async Task<UserLevel> GetById(long userId)
        {
            var user = await Table.FirstOrDefaultAsync(item => item.UserId == userId);
            return user ?? await AddNew(userId);
        }

        /* Добавление нового объекта в систему */
        private static async Task<UserLevel> AddNew(long userId)
        {
            var userLevel = new UserLevel { UserId = userId };
            var result = await Table.AddAsync(userLevel);
            await Instance.SaveChangesAsync();
            return result.Entity;
        }

        public static async Task Save()
        {
            await Instance.SaveChangesAsync();
        }
    }
}