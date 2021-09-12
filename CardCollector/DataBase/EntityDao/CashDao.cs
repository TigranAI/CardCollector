using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class CashDao
    {
        private static readonly DbSet<CashEntity> Table = CardCollectorDatabase.Instance.CashTable;
        
        public static async Task<CashEntity> GetById(long userId)
        {
            var user = await Table.FindAsync(userId);
            return user ?? await AddNew(userId);
        }

        private static async Task<CashEntity> AddNew(long userId)
        {
            var cash = new CashEntity { UserId = userId };
            var result = await Table.AddAsync(cash);
            return result.Entity;
        }
    }
}