using System.Threading.Tasks;
using CardCollector.DataBase.Entity;

namespace CardCollector.DataBase.EntityDao
{
    public static class SessionTokenDao
    {

        public static async Task AddNew(long userId, string token)
        {
            var Table = BotDatabase.Instance.SessionTokens;
            var result = await Table.AddAsync(new SessionToken {UserId = userId, Token = token});
            await BotDatabase.SaveData();
            Table.Attach(result.Entity);
            BotDatabase.Instance.ChangeTracker.Clear();
        }
    }
}