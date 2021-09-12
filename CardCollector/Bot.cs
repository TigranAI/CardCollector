using System;
using System.Timers;
using CardCollector.DataBase;
using CardCollector.Resources;
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
            var timer = new Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = Constants.SAVING_CHANGES_INTERVAL
            };
            timer.Elapsed += SavingChanges;
            Console.ReadLine();
            cts.Cancel();
        }

        private static async void SavingChanges(object o, ElapsedEventArgs e)
        {
            await CardCollectorDatabase.Instance.SaveChangesAsync();
        }
    }
}