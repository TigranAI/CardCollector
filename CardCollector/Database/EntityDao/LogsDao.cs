using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao
{
    public static class LogsDao
    {
        public static async Task<CountLogs> FindByDate(this DbSet<CountLogs> table, DateTime date)
        {
            var result = await table.Where(item => item.Date == date).SingleOrDefaultAsync();
            if (result != null) return result;
            return await AddAsync(table, date);
        }

        private static async Task<CountLogs> AddAsync(this DbSet<CountLogs> table, DateTime date)
        {
            var entity = new CountLogs(){Date = date};
            var result = await table.AddAsync(entity);
            return result.Entity;
        }
    }
}