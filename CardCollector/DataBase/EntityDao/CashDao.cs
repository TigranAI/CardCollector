using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    /* Класс, позволяющий получить доступ к объектам таблицы Cash*/
    public static class CashDao
    {
        /* Таблица cash в представлении EntityFramework */
        private static readonly DbSet<CashEntity> Table = CardCollectorDatabase.Instance.CashTable;
        
        /* Получение объекта по Id */
        public static async Task<CashEntity> GetById(long userId)
        {
            var user = await Table.FindAsync(userId);
            return user ?? await AddNew(userId);
        }

        /* Добавление нового объекта в систему */
        private static async Task<CashEntity> AddNew(long userId)
        {
            var cash = new CashEntity { UserId = userId };
            var result = await Table.AddAsync(cash);
            return result.Entity;
        }
    }
}