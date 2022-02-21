using System.Collections.Generic;
using System.Timers;

namespace CardCollector.Controllers
{
    public static class TimerController
    {
        private static LinkedList<Timer> _timers = new ();

        public static void SetupTimer(long interval, ElapsedEventHandler onElapsed)
        {
            var timer = new Timer()
            {
                Interval = interval,
                Enabled = true,
                AutoReset = false
            };
            timer.Elapsed += onElapsed;
            timer.Elapsed += (_, _) => timer.Dispose();
            timer.Disposed += (_, _) => _timers.Remove(timer);
        }
    }
}