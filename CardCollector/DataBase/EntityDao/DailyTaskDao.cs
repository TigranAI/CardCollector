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
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(DailyTaskDao));
        private static readonly DbSet<DailyTaskEntity> Table = Instance.DailyTasks;
        
        public static IAsyncEnumerable<DailyTaskEntity> GetAll()
        {
            return Table;
        }

        /* Добавляет новое отношение в таблицу */
        public static async Task<DailyTaskEntity> AddNew(long userId, int taskId)
        {
            if (await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.TaskId == taskId) is { } obj)
                return obj;
            var newTask = new DailyTaskEntity()
            {
                TaskId = taskId,
                UserId = userId,
                Progress = DailyTask.List[(DailyTaskKeys)taskId].Goal
            };
            var result = await Table.AddAsync(newTask);
            await Instance.SaveChangesAsync();
            return result.Entity;
        }

        public static async Task<DailyTaskEntity> GetTaskInfo(long userId, int taskId)
        {
            return await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.TaskId == taskId) 
                   ?? await AddNew(userId, taskId);
        }
    }
}