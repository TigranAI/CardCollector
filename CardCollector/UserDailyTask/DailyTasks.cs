using System.Collections.Generic;

namespace CardCollector.UserDailyTask
{
    public enum TaskKeys
    {
        SendStickersToUsers = 1,
    }

    public class TaskGoals
    {
        public static Dictionary<TaskKeys, int> Goals = new()
        {
            {TaskKeys.SendStickersToUsers, 5},
        };
    }
}