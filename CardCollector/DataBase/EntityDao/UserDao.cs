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
        private static CardCollectorDatabase DataBase = CardCollectorDatabase.Instance;
        
        private static Dictionary<long, UserEntity> ActiveUsers = new();
        
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
                
                ActiveUsers.Add(user.Id, user);
                return user;
            }
        }

        public static async Task<UserEntity> AddNew(User user)
        {
            var userEntity = new UserEntity();
            userEntity.Id = user.Id;
            userEntity.ChatId = user.Id;
            userEntity.Username = user.Username;
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