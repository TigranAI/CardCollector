using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class LevelDao
    {
        public static async Task<Level?> FindLevel(this DbSet<Level> table, int level)
        {
            return await table.SingleOrDefaultAsync(item => item.LevelValue == level);
        }
    }
}