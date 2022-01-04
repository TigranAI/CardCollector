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
        protected override TimeSpan RunAt => Constants.DEBUG 
            ? new TimeSpan(DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes + Constants.TEST_ALERTS_INTERVAL, 0)
            : new TimeSpan(10, 30, 0);
        
        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            var usersExp = await UserLevelDao.GetTop(5);
            if (usersExp.Count < 1) return;
            var users = await UserDao.GetAllWhere(item => !item.IsBlocked);
            var settings = await SettingsDao.GetAll();
            var messages = await UserMessagesDao.GetAll();
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
                        messages[user.Id].TopUsersId = 
                            await MessageController.DeleteAndSend(user, messages[user.Id].TopUsersId,
                                message, Keyboard.GetTopButton(TopBy.Tier4Stickers), ParseMode.Html);
                }
                catch {
                    messages[user.Id].TopUsersId = 
                        await MessageController.DeleteAndSend(user, messages[user.Id].TopUsersId,
                            message, Keyboard.GetTopButton(TopBy.Tier4Stickers), ParseMode.Html);
                }
            }
        }
    }
}