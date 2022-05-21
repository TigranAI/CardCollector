using System;
using System.Linq;
using System.Timers;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.UserDailyTask;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.TimerTasks
{
    public class DailyTaskReset : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG
            ? new TimeSpan(DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes + Constants.TEST_ALERTS_INTERVAL, 0)
            : new TimeSpan(10, 0, 0);

        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            using (var context = new BotDatabaseContext())
            {
                var users = await context.Users.Where(user => !user.IsBlocked).ToListAsync();
                foreach (var user in users)
                {
                    foreach (var userDailyTask in user.DailyTasks)
                    {
                        userDailyTask.Progress = TaskGoals.Goals[userDailyTask.TaskId];
                    }
                    if (user.Settings[Resources.Enums.UserSettingsTypes.DailyTasks])
                        await user.Messages.SendDailyTaskAlert();
                }

                await context.SaveChangesAsync();
            }
        }
    }
}