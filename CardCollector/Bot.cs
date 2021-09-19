using System;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.DataBase;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using CancellationTokenSource = System.Threading.CancellationTokenSource;

namespace CardCollector
{
    using static Controllers.MessageController;
    public static class Bot
    {
        private static TelegramBotClient _client;
        public static TelegramBotClient Client => _client ??= new TelegramBotClient(AppSettings.TOKEN);
        
        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);
            
            RunMemoryCleaner();
            
            Console.ReadLine();
            cts.Cancel();
        }

        private static void RunMemoryCleaner()
        {
            var timer = new Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = Constants.SAVING_CHANGES_INTERVAL
            };
            timer.Elapsed += SavingChanges;
        }

        private static async void SavingChanges(object o, ElapsedEventArgs e)
        {
            try
            {
                await CardCollectorDatabase.Instance.SaveChangesAsync();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}