using System;
using System.Collections.Generic;
using System.Timers;

namespace CardCollector.TimerTasks
{
    public abstract class TimerTask
    {
        protected Timer Timer = new ();
        protected abstract TimeSpan RunAt { get; }
        
        private static List<TimerTask> TasksList = new() {
            new DailyTaskAlert(),
            new DailyTaskReset(),
            new ResetChatGiveExp(),
            new ExecuteStickerEffects(),
            new PiggyBankAlert(),
            new TopExpUsersAlert()
        };

        public static void SetupAll()
        {
            foreach (var task in TasksList)
                task.Setup();
        }

        protected static void SetupTimer(Timer timer, TimeSpan timeToRun)
        {
            var interval = timeToRun - DateTime.Now.TimeOfDay;
            if (interval < TimeSpan.Zero) interval += new TimeSpan(1, 0, 0, 0);
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Interval = interval.TotalMilliseconds;
        }

        protected abstract void TimerCallback(object o, ElapsedEventArgs e);

        protected virtual void Setup()
        {
            SetupTimer(Timer, RunAt);
        }

        protected void Setup(object o, ElapsedEventArgs e)
        {
            Setup();
        }

        protected TimerTask()
        {
            Timer.Elapsed += TimerCallback;
            Timer.Elapsed += Setup;
        }
    }
}