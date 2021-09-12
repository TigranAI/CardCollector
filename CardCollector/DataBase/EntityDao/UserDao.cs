#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace CardCollector.DataBase.EntityDao
{
    public static class UserDao
    {
        private static readonly CardCollectorDatabase DataBase = CardCollectorDatabase.Instance;
        
        private static readonly Dictionary<long, UserEntity> ActiveUsers = new();

        public static async Task<UserEntity> GetOrAddNew(User user)
        {
            return await GetById(user.Id) ?? await AddNew(user);
        }
        
        public static async Task<UserEntity?> GetById(long userId)
        {
            if (!await UserExists(userId)) return null;
            try
            {
                return ActiveUsers[userId];
            }
            catch (Exception)
            {
                var user = await DataBase.Users.FindAsync(userId);
                
                // Build user object
                user.Cash = await CashDao.GetById(user.Id);
                
                ActiveUsers.Add(user.Id, user);
                return user;
            }
        }

        public static async Task<UserEntity> AddNew(User user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                ChatId = user.Id,
                Username = user.Username,
                IsBlocked = false
            };
            await DataBase.Users.AddAsync(userEntity);
            await DataBase.SaveChangesAsync();
            return userEntity;
        }

        private static async Task<bool> UserExists(long userId)
        {
            if(ActiveUsers.ContainsKey(userId)) return true;
            return await DataBase.Users.AnyAsync(e => e.Id == userId);
        }
    }
}