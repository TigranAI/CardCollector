using System;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class CountLogsDao
    {
        public static BotDatabase Instance;
        public static DbSet<CountLogs> Table;

        static CountLogsDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(CountLogsDao));
            Table = Instance.CountLogs;
        }

        public static async Task<CountLogs> Get(DateTime date)
        {
            try {
                return await Table.FirstOrDefaultAsync(item => item.Date == date) ?? await Create(date);
            }
            catch (InvalidOperationException) {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await Get(date);
            }
        }

        private static async Task<CountLogs> Create(DateTime date)
        {
            try {
                var result = (await Table.AddAsync(new CountLogs{Date = date})).Entity;
                await BotDatabase.SaveData();
                return result;
            }
            catch (InvalidOperationException) {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await Create(date);
            }
        }
    }
}