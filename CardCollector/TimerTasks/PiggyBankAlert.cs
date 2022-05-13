using System;
using System.Linq;
using System.Timers;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.TimerTasks
{
    public class PiggyBankAlert : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG
            ? new TimeSpan(DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes + Constants.TEST_ALERTS_INTERVAL, 0)
            : new TimeSpan((DateTime.Now.TimeOfDay.Hours / 4 + 1) * 4, 0, 0);

        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            using (var context = new BotDatabaseContext())
            {
                var users = await context.Users.Where(user => !user.IsBlocked).ToListAsync();
                foreach (var user in users.Where(user => user.Settings[UserSettingsTypes.PiggyBankCapacity]))
                {
                    var income = user.Cash.GetIncome(user.Stickers);
                    await user.Messages.SendPiggyBankAlert(user,$"{Messages.uncollected_income}: {income}");
                }

                await context.SaveChangesAsync();
            }
        }
    }
}