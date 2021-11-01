using System;
using System.Timers;
using CardCollector.DailyTasks;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.TimerTasks
{
    public class DailyTaskReset : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG ? new TimeSpan(12, 24, 0) : new TimeSpan(10, 0, 0);

        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            foreach (var item in await DailyTaskDao.GetAll())
                item.Progress = DailyTask.List[(DailyTaskKeys) item.TaskId].Goal;
        }
    }
}