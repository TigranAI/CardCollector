using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao
{
    public static class ShopDao
    {
        public static async Task<SpecialOrder?> FindById(this DbSet<SpecialOrder> table, int? id)
        {
            return await table.SingleOrDefaultAsync(item => item.Id == id);
        }
    }
}