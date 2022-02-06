using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class PackDao
    {
        public static async Task<Pack> FindPack(this DbSet<Pack> table, int id)
        {
            return await table.SingleAsync(pack => pack.Id == id);
        }
    }
}