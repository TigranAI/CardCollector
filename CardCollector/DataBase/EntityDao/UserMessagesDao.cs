using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class UserMessagesDao
    {
        public static BotDatabase Instance;
        public static DbSet<UserMessages> Table;

        static UserMessagesDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(UserLevelDao));
            Table = Instance.UserMessages;
        }

        /* Получение объекта по Id */
        public static async Task<UserMessages> GetById(long userId)
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
        private static async Task<UserMessages> AddNew(long userId)
        {
            try
            {
                var userLevel = new UserMessages {UserId = userId};
                var result = await Table.AddAsync(userLevel);
                return result.Entity;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await AddNew(userId);
            }
        }

        public static async Task<Dictionary<long, UserMessages>> GetAll()
        {
            try
            {
                return await Table.ToDictionaryAsync(um => um.UserId, um => um);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetAll();
            }
        }
    }
}