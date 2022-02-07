using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class UserDao
    {
        public static async Task<User> FindUser(this DbSet<User> users, Telegram.Bot.Types.User telegramUser)
        {
            return await users.SingleOrDefaultAsync(user => user.ChatId == telegramUser.Id) 
                         ?? (await users.AddAsync(new User(telegramUser))).Entity;
        }
        public static async Task<User> FindUserWithSession(this DbSet<User> users, Telegram.Bot.Types.User telegramUser)
        {
            var result = await users.FindUser(telegramUser);
            result.Session = SessionController.FindSession(result);
            return result;
        }
    }
}