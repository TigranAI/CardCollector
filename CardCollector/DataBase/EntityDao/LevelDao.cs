#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class LevelDao
    {
        public static BotDatabase Instance;
        public static DbSet<Level> Table;

        static LevelDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(LevelDao));
            Table = Instance.Levels;
        }
        
        /* Получение объекта по Id */
        public static async Task<Level?> GetLevel(int level)
        {
            try
            {
                return await Table.FirstOrDefaultAsync(item => item.LevelValue == level);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetLevel(level);
            }
        }

        /* Добавление нового объекта в систему */
        /*private static async Task<Level> AddNew(long userId)
        {
            var level = new Level();
            var result = await Table.AddAsync(level);
            await Instance.SaveChangesAsync();
            return result.Entity;
        }*/
    }
}