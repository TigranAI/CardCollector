using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao
{
    public static class ChatDistributionDao
    {
        public static async Task<ChatDistribution> FindById(this DbSet<ChatDistribution> table, int id)
        {
            return await table.SingleAsync(item => item.Id == id);
        }
    }
}