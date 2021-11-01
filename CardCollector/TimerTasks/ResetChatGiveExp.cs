using System;
using System.Timers;
using CardCollector.Commands.Message;
using CardCollector.Resources;

namespace CardCollector.TimerTasks
{
    public class ResetChatGiveExp : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG ? new TimeSpan(12, 14, 20) : new TimeSpan(10, 0, 0);
        
        protected override void TimerCallback(object o, ElapsedEventArgs e)
        {
            GiveExp.GroupStickersExp.Clear();
        }
    }
}