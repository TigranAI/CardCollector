using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class SessionTokenDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(SessionTokenDao));
        /* Таблица auction в представлении Entity Framework */
        private static readonly DbSet<SessionToken> Table = Instance.SessionTokens;

        public static async Task AddNew(long userId, string token)
        {
            var result = await Table.AddAsync(new SessionToken {UserId = userId, Token = token});
            await Instance.SaveChangesAsync();
            Table.Attach(result.Entity);
            Instance.ChangeTracker.Clear();
        }
    }
}