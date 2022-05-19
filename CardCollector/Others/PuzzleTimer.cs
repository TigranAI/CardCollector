using System;
using System.Timers;
using CardCollector.Resources;

namespace CardCollector.Others;

public class PuzzleTimer : Timer
{
    private static readonly double TURN_INTERVAL = Constants.DEBUG ? 30 * 1000 : 30 * 1000;
    
    public void Reset()
    {
        Stop();
        Start();
    }

    public static PuzzleTimer Of(Action onElapsed)
    {
        var timer = new PuzzleTimer()
        {
            AutoReset = false,
            Enabled = true,
            Interval = TURN_INTERVAL
        };
        timer.Elapsed += delegate {  onElapsed.Invoke(); };
        return timer;
    }
}