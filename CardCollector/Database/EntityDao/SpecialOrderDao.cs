using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class SpecialOrderDao
    {
        public static async Task<List<SpecialOrder>> FindAll(this DbSet<SpecialOrder> table)
        {
            var result = await table.ToListAsync();
            return result.Where(item => !item.IsExpired()).ToList();
        }
    }
}