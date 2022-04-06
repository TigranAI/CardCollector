using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao
{
    public static class PackDao
    {
        public static Task<Pack> FindById(this DbSet<Pack> table, int? id)
        {
            return table.Include(item => item.Stickers).SingleAsync(pack => pack.Id == id);
        }

        public static Task<List<Pack>> FindNext(this DbSet<Pack> table, int offset, int count)
        {
            return table.Skip(offset).Take(count).ToListAsync();
        }

        public static Task<List<Pack>> FindNextSkipRandom(this DbSet<Pack> table, int offset, int count)
        {
            return table.Where(item => item.Id != 1).Skip(offset).Take(count).ToListAsync();
        }

        public static Task<int> GetCount(this DbSet<Pack> table)
        {
            return table.CountAsync();
        }
    }
}