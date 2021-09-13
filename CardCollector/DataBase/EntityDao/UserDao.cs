using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace CardCollector.DataBase.EntityDao
{
    public static class UserDao
    {
        private static readonly DbSet<UserEntity> Table = CardCollectorDatabase.Instance.Users;
        private static readonly Dictionary<long, UserEntity> ActiveUsers = new();

        public static async Task<UserEntity> GetUser(User user)
        {
            UserEntity result;
            try
            {
                result = ActiveUsers[user.Id];
            }
            catch
            {
                result = await Table.FindAsync(user.Id) ?? await AddNew(user);
                
                // Build user object
                result.Cash = await CashDao.GetById(user.Id);
                result.Stickers = await UserStickerRelationDao.GetListById(user.Id);
                
                // Add to avoid database fetching
                ActiveUsers.Add(user.Id, result);
            }
            return result;
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
            var result = await Table.AddAsync(userEntity);
            return result.Entity;
        }
    }
}