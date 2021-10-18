using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.DailyTasks.CustomTasks;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.DailyTasks
{
    public enum DailyTaskKeys
    {
        SendStickersToUsers = 1,
    }

    public abstract class DailyTask
    {
        public static Dictionary<DailyTaskKeys, DailyTask> List = new()
        {
            {DailyTaskKeys.SendStickersToUsers, new SendStickers()}
        };
        
        public abstract int Id { get; }
        public abstract int Goal { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }

        public abstract Task<bool> Execute(long userId, object[] args = null);
        public abstract Task GiveReward(long userId, object[] args = null);

        public static async void ResetTasks(object o, ElapsedEventArgs e)
        {
            await foreach (var item in DailyTaskDao.GetAll())
                item.Progress = List[(DailyTaskKeys) item.TaskId].Goal;
            Utilities.SetUpTimer(Constants.DailyTaskReset, ResetTasks);
        }
    }
}