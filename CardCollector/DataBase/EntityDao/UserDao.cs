using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace CardCollector.DataBase.EntityDao
{
    /* Класс, предоставляющий доступ к объектам пользователей таблицы Users */
    public static class UserDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(UserDao));
        /* Таблица Users в представлении EntityFramework */
        private static readonly DbSet<UserEntity> Table = Instance.Users;
        
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

        public static async Task<UserEntity> GetById(long userId)
        {
            return await Table.FirstAsync(item => item.Id == userId);
        }

        /* Получение пользователя по представлению user из Базы данных */
        public static async Task<List<UserEntity>> GetUsersList(string filter)
        {
            return await Table.Where(user => user.Username.Contains(filter)).ToListAsync();
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
            await Instance.SaveChangesAsync();
            return result.Entity;
        }

        public static void ClearMemory(object sender, ElapsedEventArgs e)
        {
            foreach (var (id, user) in ActiveUsers)
            {
                if (user.Session.GetLastAccessInterval() <= Constants.SESSION_ACTIVE_PERIOD) continue;
                user.Session.EndSession();
                ActiveUsers.Remove(id);
            }
        }
    }
}