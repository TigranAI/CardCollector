using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.TimerTasks;

public class CountingLogs : TimerTask
{
    protected override TimeSpan RunAt => Constants.DEBUG
        ? new TimeSpan(DateTime.Now.Hour,DateTime.Now.Minute + Constants.TEST_ALERTS_INTERVAL, 0)
        : new TimeSpan(0, 0, 0);

    protected override async void TimerCallback(object o, ElapsedEventArgs e)
    {
        using (var context = new BotDatabaseContext())
        {
            await RemoveOldActivities(context);

            for (var day = -7; day < 0; day++)
            {
                var leftBorder = DateTime.Today.AddDays(day);
                var activityResults = await context.CountLogs.FindByDate(leftBorder);
                if (!activityResults.IsEmpty()) continue;

                var rightBorder = leftBorder.AddDays(1);
                var dateActivities = await context.UserActivities
                    .Where(item => item.Timestamp >= leftBorder && item.Timestamp < rightBorder)
                    .ToListAsync();
                if (dateActivities.Count == 0) continue;

                Functions.CalculateActivityResults(activityResults, dateActivities);
            }

            await context.SaveChangesAsync();
        }
    }

    private static async Task RemoveOldActivities(BotDatabaseContext context)
    {
        var dayAWeekAgo = DateTime.Today.AddDays(-7);
        var oldActivities = await context.UserActivities
            .Where(item => item.Timestamp <= dayAWeekAgo)
            .ToListAsync();
        context.AttachRange(oldActivities);
        context.RemoveRange(oldActivities);
    }
}