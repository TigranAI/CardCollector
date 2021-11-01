using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class UserLevelDao
    {
        /* Получение объекта по Id */
        public static async Task<UserLevel> GetById(long userId)
        {
            var Table = BotDatabase.Instance.UserLevel;
            var user = await Table.FirstOrDefaultAsync(item => item.UserId == userId);
            return user ?? await AddNew(userId);
        }

        /* Добавление нового объекта в систему */
        private static async Task<UserLevel> AddNew(long userId)
        {
            var Table = BotDatabase.Instance.UserLevel;
            var userLevel = new UserLevel { UserId = userId };
            var result = await Table.AddAsync(userLevel);
            await BotDatabase.SaveData();
            return result.Entity;
        }

        public static Task<List<UserLevel>> GetTop(int top)
        {
            var Table = BotDatabase.Instance.UserLevel;
            return Table.OrderByDescending(item => item.TotalExp).Take(top).ToListAsync();
        }
    }
}