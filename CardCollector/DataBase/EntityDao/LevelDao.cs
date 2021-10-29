#nullable enable
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class LevelDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(UserLevelDao));
        /* Таблица cash в представлении EntityFramework */
        private static readonly DbSet<Level> Table = Instance.Levels;
        
        /* Получение объекта по Id */
        public static async Task<Level?> GetLevel(int level)
        {
            return await Table.FirstOrDefaultAsync(item => item.LevelValue == level);
        }

        /* Добавление нового объекта в систему */
        /*private static async Task<Level> AddNew(long userId)
        {
            var level = new Level();
            var result = await Table.AddAsync(level);
            await Instance.SaveChangesAsync();
            return result.Entity;
        }*/

        public static async Task Save()
        {
            await Instance.SaveChangesAsync();
        }
    }
}