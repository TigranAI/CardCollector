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
            while (interval < TimeSpan.Zero) interval += new TimeSpan(1, 0, 0, 0);
            SetupTimer(timer, interval.TotalMilliseconds);
        }

        protected static void SetupTimer(Timer timer, double interval)
        {
            if (interval < int.MaxValue)
            {
                timer.AutoReset = false;
                timer.Enabled = true;
                timer.Interval = interval;
            }
            else
            {
                var t = new Timer()
                {
                    Interval = int.MaxValue,
                    AutoReset = false,
                    Enabled = true
                };
                t.Elapsed += delegate { SetupTimer(timer, interval - int.MaxValue); };
            }
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