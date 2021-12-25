using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DailyTasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class DailyTaskDao
    {
        public static BotDatabase Instance;
        public static DbSet<DailyTaskEntity> Table;

        static DailyTaskDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(DailyTaskDao));
            Table = Instance.DailyTasks;
        }
        
        public static async Task<List<DailyTaskEntity>> GetAll()
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

        /* Добавляет новое отношение в таблицу */
        public static async Task<DailyTaskEntity> AddNew(long userId, int taskId)
        {
            try
            {
                if (await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.TaskId == taskId) is { } obj)
                    return obj;
                var newTask = new DailyTaskEntity()
                {
                    TaskId = taskId,
                    UserId = userId,
                    Progress = DailyTask.List[(DailyTaskKeys) taskId].Goal
                };
                var result = await Table.AddAsync(newTask);
                await Instance.SaveChangesAsync();
                return result.Entity;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await AddNew(userId, taskId);
            }
        }

        public static async Task<DailyTaskEntity> GetTaskInfo(long userId, int taskId)
        {
            try
            {
                return await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.TaskId == taskId)
                       ?? await AddNew(userId, taskId);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetTaskInfo(userId, taskId);
            }
        }

        public static async Task<Dictionary<int, DailyTaskEntity>> GetUserTasks(long userId)
        {
            try
            {
                return await Table.Where(item => item.UserId == userId).ToDictionaryAsync(p => p.TaskId, p => p);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetUserTasks(userId);
            }
        }
    }
}