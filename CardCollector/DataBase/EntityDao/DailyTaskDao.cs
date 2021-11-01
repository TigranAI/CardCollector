using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DailyTasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class DailyTaskDao
    {
        public static async Task<List<DailyTaskEntity>> GetAll()
        {
            var Table = BotDatabase.Instance.DailyTasks;
            return await Table.ToListAsync();
        }

        /* Добавляет новое отношение в таблицу */
        public static async Task<DailyTaskEntity> AddNew(long userId, int taskId)
        {
            var Table = BotDatabase.Instance.DailyTasks;
            if (await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.TaskId == taskId) is { } obj)
                return obj;
            var newTask = new DailyTaskEntity()
            {
                TaskId = taskId,
                UserId = userId,
                Progress = DailyTask.List[(DailyTaskKeys)taskId].Goal
            };
            var result = await Table.AddAsync(newTask);
            await BotDatabase.SaveData();
            return result.Entity;
        }

        public static async Task<DailyTaskEntity> GetTaskInfo(long userId, int taskId)
        {
            var Table = BotDatabase.Instance.DailyTasks;
            return await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.TaskId == taskId)
                   ?? await AddNew(userId, taskId);
        }

        public static async Task<Dictionary<int, DailyTaskEntity>> GetUserTasks(long userId)
        {
            var Table = BotDatabase.Instance.DailyTasks;
            return await Table.Where(item => item.UserId == userId).ToDictionaryAsync(p => p.TaskId, p => p);
        }
    }
}