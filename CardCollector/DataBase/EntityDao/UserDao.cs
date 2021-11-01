using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace CardCollector.DataBase.EntityDao
{
    /* Класс, предоставляющий доступ к объектам пользователей таблицы Users */
    public static class UserDao
    {
        /* Активные пользователи в системе */
        private static readonly Dictionary<long, UserEntity> ActiveUsers = new();

        /* Получение пользователя по представлению user из Базы данных */
        public static async Task<UserEntity> GetUser(User user)
        {
            UserEntity result;
            try
            {
                /* Пытаемся получить пользователя из списка активных */
                result = ActiveUsers[user.Id];
            }
            catch
            {
                var Table = BotDatabase.Instance.Users;
                /* Ищем пользователя в базе данных или добавляем нового, если не найден*/
                result = await Table.FindAsync(user.Id) ?? await AddNew(user);
                
                /* Собираем объект пользователя */
                result.Cash = await CashDao.GetById(user.Id);
                result.Stickers = await UserStickerRelationDao.GetListById(user.Id);
                result.Settings = await SettingsDao.GetById(user.Id);
                result.CurrentLevel = await UserLevelDao.GetById(user.Id);
                result.Session.InitNewModule<FiltersModule>();
                result.Session.InitNewModule<DefaultModule>();
                
                /* Добавляем пользователя в список активных, чтобы не обращаться к бд лишний раз */
                ActiveUsers.Add(user.Id, result);
            }
            /* Обновляем имя пользователя, если он его сменил */
            if (result.Username != user.Username && user.Username != "") result.Username = user.Username;
            return result;
        }

        public static async Task<UserEntity> GetById(long userId)
        {
            var Table = BotDatabase.Instance.Users;
            var user = await Table.FirstAsync(item => item.Id == userId);
            user.Cash = await CashDao.GetById(user.Id);
            //user.Stickers = await UserStickerRelationDao.GetListById(user.Id);
            return user;
        }

        /* Получение пользователя по представлению user из Базы данных */
        public static async Task<List<UserEntity>> GetUsersList(string filter)
        {
            var Table = BotDatabase.Instance.Users;
            return await Table.Where(user => user.Username.Contains(filter)).ToListAsync();
        }

        /* Добавление новго пользователя в систему */
        private static async Task<UserEntity> AddNew(User user)
        {
            var Table = BotDatabase.Instance.Users;
            var userEntity = new UserEntity
            {
                Id = user.Id,
                ChatId = user.Id,
                Username = user.Username != "" ? user.Username : "user"+user.Id,
                IsBlocked = false
            };
            var result = await Table.AddAsync(userEntity);
            await BotDatabase.SaveData();
            return result.Entity;
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

        public static Task<List<UserEntity>> GetAllWhere(Expression<Func<UserEntity, bool>> callback)
        {
            var Table = BotDatabase.Instance.Users;
            return Table.Where(callback).ToListAsync();
        }

        public static async Task<IEnumerable<UserEntity>> GetAll()
        {
            var Table = BotDatabase.Instance.Users;
            return await Table.ToListAsync();
        }
    }
}