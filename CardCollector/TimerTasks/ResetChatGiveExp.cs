using System;
using System.Timers;
using CardCollector.Commands.Message;
using CardCollector.Resources;

namespace CardCollector.TimerTasks
{
    public class ResetChatGiveExp : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG 
            ? new TimeSpan(DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes + Constants.TEST_ALERTS_INTERVAL, 0)
            : new TimeSpan(10, 0, 0);
        
        protected override void TimerCallback(object o, ElapsedEventArgs e)
        {
            GiveExp.GroupStickersExp.Clear();
        }
    }
}