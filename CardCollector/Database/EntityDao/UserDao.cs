using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Resources.Enums;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao
{
    public static class UserDao
    {
        public static async Task<User> FindUser(this DbSet<User> users, Telegram.Bot.Types.User telegramUser)
        {
            var result = await users.SingleOrDefaultAsync(user => user.ChatId == telegramUser.Id)
                         ?? (await users.AddAsync(new User(telegramUser))).Entity.SetNew();
            if (telegramUser.Username is { } username && result.Username != username) result.Username = username;
            return result;
        }

        public static async Task<User?> FindById(this DbSet<User> users, long userId)
        {
            return await users
                .SingleOrDefaultAsync(user => user.Id == userId);
        }

        public static async Task<User?> FindByChatId(this DbSet<User> users, long chatId)
        {
            return await users
                .SingleOrDefaultAsync(user => user.ChatId == chatId);
        }

        public static async Task<List<User>> FindTopByExp(this DbSet<User> users, int top = 5)
        {
            return await users
                .Where(user => user.PrivilegeLevel < PrivilegeLevel.Tester)
                .OrderByDescending(user => user.Level.TotalExp)
                .Take(top)
                .ToListAsync();
        }

        public static async Task<List<User>> FindTopByTier4Stickers(this DbSet<User> users, int top = 5)
        {
            return await users
                .Where(user => user.PrivilegeLevel < PrivilegeLevel.Tester)
                .OrderByDescending(user =>
                    user.Stickers.Where(sticker => sticker.Sticker.Tier == 4).Sum(item => item.Count))
                .Take(top)
                .ToListAsync();
        }

        public static async Task<List<long>> SelectUserChatIds(this DbSet<User> table)
        {
            var users = await table
                .Where(item => !item.IsBlocked)
                .ToListAsync();
            return users
                .Where(item => item.Settings[UserSettingsTypes.Distributions])
                .Select(item => item.ChatId)
                .ToList();
        }
    }
}