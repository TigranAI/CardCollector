using System;
using System.Linq;
using System.Timers;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;

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
            using (var context = new BotDatabaseContext())
            {
                var topByExp = await context.Users.FindTopByExp();
                var users = await context.Users.Where(user => !user.IsBlocked).ToListAsync();
                if (topByExp.Count != 0)
                {
                    var message = Messages.users_top_exp + string.Join("\n", topByExp.Select((user, i) =>
                        $"\n{i + 1}.{user.Username}: {user.Level.TotalExp} {Text.exp}"));
                    foreach (var user in users.Where(user => user.Settings[UserSettingsEnum.DailyExpTop]))
                        await user.Messages.SendTopUsers(user, message, Keyboard.GetTopButton(TopBy.Tier4Stickers));
                }

                await context.SaveChangesAsync();
            }
        }
    }
}