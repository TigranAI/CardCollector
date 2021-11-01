using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DailyTasks.CustomTasks;
using CardCollector.DataBase.Entity;

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

        public abstract Task<bool> Execute(UserEntity user, object[] args = null);
        public abstract Task GiveReward(UserEntity user, object[] args = null);
    }
}