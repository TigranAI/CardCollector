using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace CardCollector.DataBase.EntityDao
{
    /* Класс, предоставляющий доступ к объектам пользователей таблицы Users */
    public static class UserDao
    {
        /* Таблица Users в представлении EntityFramework */
        private static readonly DbSet<UserEntity> Table = CardCollectorDatabase.Instance.Users;
        
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
                /* Ищем пользователя в базе данных или добавляем нового, если не найден*/
                result = await Table.FindAsync(user.Id) ?? await AddNew(user);
                
                /* Собираем объект пользователя */
                result.Cash = await CashDao.GetById(user.Id);
                result.Stickers = await UserStickerRelationDao.GetListById(user.Id);
                
                /* Добавляем пользователя в список активных, чтобы не обращаться к бд лишний раз */
                ActiveUsers.Add(user.Id, result);
            }
            /* Обновляем имя пользователя, если он его сменил */
            result.Username = user.Username;
            return result;
        }

        /* Добавление новго пользователя в систему */
        private static async Task<UserEntity> AddNew(User user)
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