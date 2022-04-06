using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao
{
    public static class InviteInfoDao
    {
        public static Task<InviteInfo?> FindByToken(this DbSet<InviteInfo> table, string token)
        {
            return table.SingleOrDefaultAsync(item => item.InviteKey == token);
        }
    }
}