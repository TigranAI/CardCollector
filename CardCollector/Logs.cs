using System;
using System.Globalization;
// ReSharper disable LocalizableElement

namespace CardCollector
{
    public static class Logs
    {
        public static void LogOut(object message)
        {
            Console.WriteLine($"[INFO] [{DateTime.Now.ToString(CultureInfo.CurrentCulture)}] {message}");
        }
        public static void LogOutWarning(object message)
        {
            Console.WriteLine($"[WARNING] [{DateTime.Now.ToString(CultureInfo.CurrentCulture)}] {message}");
        }
        public static void LogOutError(object message)
        {
            Console.WriteLine($"[ERROR] [{DateTime.Now.ToString(CultureInfo.CurrentCulture)}] {message}");
        }
    }
}