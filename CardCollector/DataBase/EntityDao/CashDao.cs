using System;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    /* Класс, позволяющий получить доступ к объектам таблицы Cash*/
    public static class CashDao
    {
        public static BotDatabase Instance;
        public static DbSet<CashEntity> Table;

        static CashDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(CashDao));
            Table = Instance.Cash;
        }
        
        /* Получение объекта по Id */
        public static async Task<CashEntity> GetById(long userId)
        {
            try
            {
                var user = await Table.FindAsync(userId);
                return user ?? await AddNew(userId);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetById(userId);
            }
        }

        /* Добавление нового объекта в систему */
        private static async Task<CashEntity> AddNew(long userId)
        {
            try
            {
                var cash = new CashEntity {UserId = userId};
                var result = await Table.AddAsync(cash);
                await Instance.SaveChangesAsync();
                return result.Entity;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await AddNew(userId);
            }
        }
    }
}