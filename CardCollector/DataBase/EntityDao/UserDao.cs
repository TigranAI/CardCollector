using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace CardCollector.DataBase.EntityDao
{
    /* Класс, предоставляющий доступ к объектам пользователей таблицы Users */
    public static class UserDao
    {
        public static BotDatabase Instance;
        public static DbSet<UserEntity> Table;

        static UserDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(UserDao));
            Table = Instance.Users;
        }

        /* Активные пользователи в системе */
        private static readonly Dictionary<long, UserEntity> ActiveUsers = new();

        /* Получение пользователя по представлению user из Базы данных */
        public static async Task<UserEntity> GetUser(User user)
        {
            UserEntity result;
            if (ActiveUsers.ContainsKey(user.Id))
                result = ActiveUsers[user.Id];
            else
            {
                try
                {
                    result = await Table.FirstOrDefaultAsync(item => item.Id == user.Id) ?? await AddNew(user);
                }
                catch (InvalidOperationException)
                {
                    Thread.Sleep(Utilities.rnd.Next(30));
                    return await GetUser(user);
                }
                /* Собираем объект пользователя */
                result.Cash = await CashDao.GetById(user.Id);
                result.Stickers = await UserStickerRelationDao.GetListById(user.Id);
                result.Settings = await SettingsDao.GetById(user.Id);
                result.CurrentLevel = await UserLevelDao.GetById(user.Id);
                result.MessagesId = await UserMessagesDao.GetById(user.Id);
                /* Добавляем пользователя в список активных, чтобы не обращаться к бд лишний раз */
                ActiveUsers.Add(user.Id, result);
            }
            /* Обновляем имя пользователя, если он его сменил */
            if (user.Username != null && result.Username != user.Username) result.Username = user.Username;
            return result;
        }

        public static async Task<UserEntity> GetById(long userId)
        {
            try
            {
                return await Table.FirstOrDefaultAsync(item => item.Id == userId);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetById(userId);
            }
        }

        /* Получение пользователя по представлению user из Базы данных */
        public static async Task<List<UserEntity>> GetUsersList(string filter)
        {
            try
            {
                return await Table.Where(user => user.Username.Contains(filter)).ToListAsync();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetUsersList(filter);
            }
        }

        /* Добавление новго пользователя в систему */
        private static async Task<UserEntity> AddNew(User user)
        {
            try
            {
                var userEntity = new UserEntity
                {
                    Id = user.Id,
                    ChatId = user.Id,
                    Username = user.Username == null ? "user"+user.Id : user.Username,
                    IsBlocked = false
                };
                var result = await Table.AddAsync(userEntity);
                await Instance.SaveChangesAsync();
                return result.Entity;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await AddNew(user);
            }
        }

        public static async void ClearMemory(object sender, ElapsedEventArgs e)
        {
            foreach (var (id, user) in ActiveUsers)
            {
                if (user.Session.GetLastAccessInterval() <= Constants.SESSION_ACTIVE_PERIOD) continue;
                await user.Session.EndSession();
                ActiveUsers.Remove(id);
            }
        }

        public static async Task ClearMemory()
        {
            foreach (var (id, user) in ActiveUsers.ToDictionary(pair => pair.Key, pair => pair.Value))
            {
                await user.Session.EndSession();
                ActiveUsers.Remove(id);
                await MessageController.EditMessage(user, Messages.bot_turning_off);
            }
        }

        public static async Task<List<UserEntity>> GetAllWhere(Expression<Func<UserEntity, bool>> callback)
        {
            try
            {
                return await Table.Where(callback).ToListAsync();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetAllWhere(callback);
            }
        }

        public static async Task<IEnumerable<UserEntity>> GetAll()
        {
            
            try
            {
                return await Table.ToListAsync();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetAll();
            }
        }
    }
}