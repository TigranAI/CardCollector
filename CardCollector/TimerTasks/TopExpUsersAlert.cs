using System;
using System.Timers;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types.Enums;

namespace CardCollector.TimerTasks
{
    public class TopExpUsersAlert : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG ? new TimeSpan(10, 17, 20) : new TimeSpan(24, 0, 0);
        
        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            var usersExp = await UserLevelDao.GetTop(5);
            var users = await UserDao.GetAllWhere(item => !item.IsBlocked);
            var settings = await SettingsDao.GetAll();
            var message = Messages.users_top_exp;
            foreach (var (userLevel, index) in usersExp.WithIndex())
            {
                var user = await UserDao.GetById(userLevel.UserId);
                message += $"\n{index+1}.{user.Username}: {userLevel.TotalExp} {Text.exp}";
            }
            foreach (var user in users)
            {
                try {
                    if (settings[user.Id][UserSettingsEnum.DailyExpTop])
                        await MessageController.SendMessage(user, message, parseMode: ParseMode.Html, addToList: false);
                }
                catch {
                    await MessageController.SendMessage(user, message, parseMode: ParseMode.Html, addToList: false);
                }
            }
        }
    }
}