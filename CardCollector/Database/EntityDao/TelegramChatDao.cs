using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class TelegramChatDao
    {
        public static async Task<TelegramChat?> FindById(this DbSet<TelegramChat> table, int id)
        {
            return await table.SingleOrDefaultAsync(item => item.Id == id);
        }
    }
}