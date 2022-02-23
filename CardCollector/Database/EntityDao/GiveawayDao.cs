using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao
{
    public static class GiveawayDao
    {
        public static async Task<ChannelGiveaway?> FindById(this DbSet<ChannelGiveaway> table, int? id)
        {
            return await table.SingleOrDefaultAsync(item => item.Id == id);
        }

        public static async Task<ChannelGiveaway> CreateNew(this DbSet<ChannelGiveaway> table)
        {
            var result = await table.AddAsync(new ChannelGiveaway());
            return result.Entity;
        }
    }
}