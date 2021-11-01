using System.Threading.Tasks;
using CardCollector.DataBase.Entity;

namespace CardCollector.DataBase.EntityDao
{
    /* Класс, позволяющий получить доступ к объектам таблицы Cash*/
    public static class CashDao
    {
        /* Получение объекта по Id */
        public static async Task<CashEntity> GetById(long userId)
        {
            var Table = BotDatabase.Instance.Cash;
            var user = await Table.FindAsync(userId);
            return user ?? await AddNew(userId);
        }

        /* Добавление нового объекта в систему */
        private static async Task<CashEntity> AddNew(long userId)
        {
            var Table = BotDatabase.Instance.Cash;
            var cash = new CashEntity { UserId = userId };
            var result = await Table.AddAsync(cash);
            await BotDatabase.SaveData();
            return result.Entity;
        }
    }
}