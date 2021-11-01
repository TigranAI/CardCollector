using System;
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

        public static async Task AddNew(long userId, string token)
        {
            try
            {
                var result = await Table.AddAsync(new SessionToken {UserId = userId, Token = token});
                await BotDatabase.SaveData();
                Table.Attach(result.Entity);
                Instance.ChangeTracker.Clear();
            }
            catch (InvalidOperationException)
            {
                Logs.LogOut("Cant creat token");
            }
        }
    }
}