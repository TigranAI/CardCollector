using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao
{
    public static class ChatRouletteDao
    {
        public static async Task<ChatRoulette?> FindById(this DbSet<ChatRoulette> table, long id)
        {
            return await table.SingleOrDefaultAsync(item => item.Id == id);
        }
    }
}