using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
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
        
        public static async Task<User> FindByIdWithSession(this DbSet<User> users, long userId)
        {
            var result = await users.SingleAsync(user => user.Id == userId);
            result.Session = SessionController.FindSession(result);
            return result;
        }
        
        public static async Task<User> FindUserWithSession(this DbSet<User> users, Telegram.Bot.Types.User telegramUser)
        {
            var result = await users.FindUser(telegramUser);
            result.Session = SessionController.FindSession(result);
            return result;
        }

        public static async Task<List<User>> FindTopByExp(this DbSet<User> users, int top = 5)
        {
            return await users
                .Include(user => user.Level)
                .Where(user => user.PrivilegeLevel < PrivilegeLevel.Programmer)
                .OrderByDescending(user => user.Level.TotalExp)
                .Take(top)
                .ToListAsync();
        }

        public static async Task<List<User>> FindTopByTier4Stickers(this DbSet<User> users, int top = 5)
        {
            return await users
                .Include(user => user.Stickers)
                .Include(user => user.Stickers.Select(sticker => sticker.Sticker))
                .Where(user => user.PrivilegeLevel < PrivilegeLevel.Programmer)
                .OrderByDescending(user => user.Stickers.Where(sticker => sticker.Sticker.Tier == 4).Count())
                .Take(top)
                .ToListAsync();
        }
    }
}