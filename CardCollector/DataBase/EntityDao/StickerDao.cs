using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class StickerDao
    {
        public static async Task<Sticker[]> FindAllByTier(this DbSet<Sticker> table, int tier)
        {
            return await table.Where(item => item.Tier == tier).ToArrayAsync();
        }
    }
}