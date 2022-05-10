using System;
using System.Linq;
using System.Timers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
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
            /*using (var context = new BotDatabaseContext())
            {
                var topByExp = await context.Users.FindTopByExp();
                var users = await context.Users.Where(user => !user.IsBlocked).ToListAsync();
                if (topByExp.Count != 0)
                {
                    var message = Messages.users_top_exp + string.Join("\n", topByExp.Select((user, i) =>
                        $"\n{i + 1}.{user.Username}: {user.Level.TotalExp} {Text.exp}"));
                    foreach (var user in users.Where(user => user.Settings[UserSettingsTypes.DailyExpTop]))
                        await user.Messages.SendTopUsers(user, message, Keyboard.GetTopButton(TopBy.Tier4Stickers));
                }

                await context.SaveChangesAsync();
            }*/
        }
    }
}