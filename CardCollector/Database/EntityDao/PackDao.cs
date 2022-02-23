using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao
{
    public static class PackDao
    {
        public static async Task<Pack> FindById(this DbSet<Pack> table, int? id)
        {
            return await table.Include(item => item.Stickers).SingleAsync(pack => pack.Id == id);
        }

        public static async Task<List<Pack>> FindNext(this DbSet<Pack> table, int offset, int count)
        {
            return await table.Skip(offset).Take(count).ToListAsync();
        }

        public static async Task<List<Pack>> FindNextSkipRandom(this DbSet<Pack> table, int offset, int count)
        {
            return await table.Where(item => item.Id != 1).Skip(offset).Take(count).ToListAsync();
        }

        public static async Task<int> GetCount(this DbSet<Pack> table)
        {
            return await table.CountAsync();
        }
    }
}