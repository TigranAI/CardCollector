using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class GiveawayDao
    {
        public static async Task<ChannelGiveaway> FindById(this DbSet<ChannelGiveaway> table, int id)
        {
            return await table.SingleAsync(item => item.Id == id);
        }

        public static async Task<ChannelGiveaway> CreateNew(this DbSet<ChannelGiveaway> table)
        {
            var result = await table.AddAsync(new ChannelGiveaway());
            return result.Entity;
        }
    }
}