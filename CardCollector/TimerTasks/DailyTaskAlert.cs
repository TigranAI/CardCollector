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
        protected override TimeSpan RunAt => Constants.DEBUG ? new TimeSpan(12, 24, 0) : new TimeSpan(10, 0, 0);
        
        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            var users = await UserDao.GetAllWhere(user => !user.IsBlocked);
            var settings = await SettingsDao.GetAll();
            foreach (var user in users)
            {
                try {
                    if (settings[user.Id][UserSettingsEnum.DailyTasks])
                        await MessageController.SendMessage(user, Messages.daily_task_alertation, addToList: false);
                }
                catch {
                    await MessageController.SendMessage(user, Messages.daily_task_alertation, addToList: false);
                }
            }
        }
    }
}