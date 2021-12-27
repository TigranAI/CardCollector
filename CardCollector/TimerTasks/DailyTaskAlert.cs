using System;
using System.Timers;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.TimerTasks
{
    public class DailyTaskAlert : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG
            ? new TimeSpan(DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes + Constants.TEST_ALERTS_INTERVAL, 0)
            : new TimeSpan(10, 0, 0);

        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            var users = await UserDao.GetAllWhere(user => !user.IsBlocked);
            var settings = await SettingsDao.GetAll();
            var messages = await UserMessagesDao.GetAll();
            foreach (var user in users)
            {
                try
                {
                    if (settings[user.Id][UserSettingsEnum.DailyTasks])
                        messages[user.Id].DailyTaskId =
                            await MessageController.DeleteAndSend(user, messages[user.Id].DailyTaskId,
                                Messages.daily_task_alertation);
                }
                catch
                {
                    messages[user.Id].DailyTaskId =
                        await MessageController.DeleteAndSend(user, messages[user.Id].DailyTaskId,
                            Messages.daily_task_alertation);
                }
            }
        }
    }
}