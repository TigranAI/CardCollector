using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class UserLevelDao
    {
        public static BotDatabase Instance;
        public static DbSet<UserLevel> Table;

        static UserLevelDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(UserLevelDao));
            Table = Instance.UserLevel;
        }
        
        /* Получение объекта по Id */
        public static async Task<UserLevel> GetById(long userId)
        {
            try
            {
                var user = await Table.FirstOrDefaultAsync(item => item.UserId == userId);
                return user ?? await AddNew(userId);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetById(userId);
            }
        }

        /* Добавление нового объекта в систему */
        private static async Task<UserLevel> AddNew(long userId)
        {
            try
            {
                var userLevel = new UserLevel { UserId = userId };
                var result = await Table.AddAsync(userLevel);
                await Instance.SaveChangesAsync();
                return result.Entity;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await AddNew(userId);
            }
        }

        public static async Task<List<UserLevel>> GetTop(int top)
        {
            try
            {
                return await Table.OrderByDescending(item => item.TotalExp).Take(top).ToListAsync();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetTop(top);
            }
        }
    }
}