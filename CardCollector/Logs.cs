using System;
using System.IO;
using CardCollector.Resources;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable LocalizableElement
#pragma warning disable 162

namespace CardCollector
{
    public static class Logs
    {
        private static void Print(string message)
        {
            if (Constants.DEBUG) Console.WriteLine(message);
            else File.AppendAllText($"Log {DateTime.Now.Date}.txt", message + Environment.NewLine);
        }
        
        public static void LogOut(object message)
        {
            Print($"[INFO] [{DateTime.Now.TimeOfDay.ToString()}] {message}");
        }
        public static void LogOutWarning(object message)
        {
            Print($"[WARNING] [{DateTime.Now.TimeOfDay.ToString()}] {message}");
        }
        public static void LogOutError(object message)
        {
            Print($"[ERROR] [{DateTime.Now.TimeOfDay.ToString()}] {message}");
        }

        public static void LogOutJson(object message)
        {
            Print($"[JSON] [{DateTime.Now.TimeOfDay.ToString()}] {Utilities.ToJson(message)}");
        }
    }
}