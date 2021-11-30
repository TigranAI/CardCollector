using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class SessionTokenDao
    {
        public static BotDatabase Instance;
        public static DbSet<SessionToken> Table;

        static SessionTokenDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(SessionTokenDao));
            Table = Instance.SessionTokens;
        }

        public static async Task<string> AddNew(long userId, string token)
        {
            try
            {
                var result = await Table.FirstOrDefaultAsync(item => item.UserId == userId) ??
                    (await Table.AddAsync(new SessionToken {UserId = userId, Token = token})).Entity;
                await Instance.SaveChangesAsync();
                Table.Attach(result);
                Instance.ChangeTracker.Clear();
                return result.Token;
            }
            catch (InvalidOperationException)
            {
                Logs.LogOut("Cant creat token");
                return "";
            }
        }
    }
}