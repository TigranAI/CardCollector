using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class SettingsDao
    {
        public static async Task<UserSettings> GetById(long userId)
        {
            var Table = BotDatabase.Instance.Settings;
            return await Table.FirstOrDefaultAsync(item => item.UserId == userId) ?? await AddNew(userId);
        }
        
        public static async Task<UserSettings> AddNew(long userId)
        {
            var Table = BotDatabase.Instance.Settings;
            var entry = new UserSettings {UserId = userId};
            entry.InitProperties();
            var result = await Table.AddAsync(entry);
            return result.Entity;
        }

        public static async Task<Dictionary<long, UserSettings>> GetAll()
        {
            var Table = BotDatabase.Instance.Settings;
            return await Table.ToDictionaryAsync(item => item.UserId, item => item);
        }
    }
}