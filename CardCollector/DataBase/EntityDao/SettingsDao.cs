using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class SettingsDao
    {
        public static BotDatabase Instance;
        public static DbSet<UserSettings> Table;

        static SettingsDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(SettingsDao));
            Table = Instance.Settings;
        }
        
        public static async Task<UserSettings> GetById(long userId)
        {
            try
            {
                return await Table.FirstOrDefaultAsync(item => item.UserId == userId) ?? await AddNew(userId);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetById(userId);
            }
        }
        
        public static async Task<UserSettings> AddNew(long userId)
        {
            try
            {
                var entry = new UserSettings {UserId = userId};
                entry.InitProperties();
                var result = await Table.AddAsync(entry);
                return result.Entity;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await AddNew(userId);
            }
        }

        public static async Task<Dictionary<long, UserSettings>> GetAll()
        {
            try
            {
                return await Table.ToDictionaryAsync(item => item.UserId, item => item);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetAll();
            }
        }
    }
}