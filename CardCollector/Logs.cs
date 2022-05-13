using System;
using System.IO;
using System.Reflection;
using CardCollector.Resources;
using Newtonsoft.Json;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable LocalizableElement
#pragma warning disable 162

namespace CardCollector
{
    public static class Logs
    {
        private static readonly string Path;

        static Logs()
        {
            Path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "/Logs";
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        private static async void Print(string message)
        {
            if (Constants.DEBUG) Console.WriteLine(message);
            else
            {
                using (var sw =
                    File.AppendText($"{Path}/{DateTime.Today.ToString(Constants.TimeCulture.ShortDatePattern)}.log"))
                {
                    await sw.WriteLineAsync(message);
                    sw.Close();
                }
            }
        }

        public static void LogOut(object message)
        {
            Print($"[INFO] [{DateTime.Now.ToString(Constants.TimeCulture.LongTimePattern)}] {message}");
        }

        public static void LogOutWarning(object message)
        {
            Print($"[WARNING] [{DateTime.Now.ToString(Constants.TimeCulture.LongTimePattern)}] {message}");
        }

        public static void LogOutError(object message)
        {
            Print($"[ERROR] [{DateTime.Now.ToString(Constants.TimeCulture.LongTimePattern)}] {message}");
        }

        public static void LogOutJson(object message)
        {
            Print($"[JSON] [{DateTime.Now.ToString(Constants.TimeCulture.LongTimePattern)}] " +
                  $"{Utilities.ToJson(message, Formatting.Indented)}");
        }
    }
}