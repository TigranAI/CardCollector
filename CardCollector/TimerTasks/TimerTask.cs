using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;

namespace CardCollector.TimerTasks
{
    public abstract class TimerTask
    {
        protected Timer Timer = new ();
        protected abstract TimeSpan RunAt { get; }

        private static ICollection<TimerTask?> TasksList = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(TimerTask)))
            .Select(type => (TimerTask?) Activator.CreateInstance(type))
            .ToList();

        public static void SetupAll()
        {
            foreach (var task in TasksList)
                task?.Setup();
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

        public TimerTask()
        {
            Timer.Elapsed += TimerCallback;
            Timer.Elapsed += Setup;
        }
    }
}