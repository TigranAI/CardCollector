using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class ShopDao
    {
        public static async Task<SpecialOrder?> FindById(this DbSet<SpecialOrder> table, long? id)
        {
            return await table.FirstOrDefaultAsync(item => item.Id == id);
        }
    }
}