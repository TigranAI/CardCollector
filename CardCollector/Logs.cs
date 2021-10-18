using System;
using System.IO;
using System.Reflection;
using CardCollector.Resources;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable LocalizableElement
#pragma warning disable 162

namespace CardCollector
{
    public static class Logs
    {
        private static readonly string path;

        static Logs()
        {
            path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + @"\Logs";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        
        private static async void Print(string message)
        {
            var today = DateTime.Today;
            if (Constants.DEBUG) Console.WriteLine(message);
            else
            {
                using(var sw = File.AppendText($@"{path}\{today.Day}-{today.Month}-{today.Year}.log"))
                    await sw.WriteLineAsync(message);
            }
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