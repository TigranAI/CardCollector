using System;
using System.Timers;
using CardCollector.Cache.Repository;
using CardCollector.Resources;

namespace CardCollector.TimerTasks
{
    public class ResetChatRestrictions : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG 
            ? new TimeSpan(DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes + Constants.TEST_ALERTS_INTERVAL, 0)
            : new TimeSpan(10, 0, 0);
        
        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            await new ChatInfoRepository().ClearAsync();
            await new LadderInfoRepository().ClearAsync();
            await new UserInfoRepository().ClearAsync();
        }
    }
}