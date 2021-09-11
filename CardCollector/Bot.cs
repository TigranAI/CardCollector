using System;
using System.Threading;
using Telegram.Bot;
using static CardCollector.Controllers.MessageController;

namespace CardCollector
{
    using static Resources.AppSettings;
    public static class Bot
    {
        private static TelegramBotClient _client;
        public static TelegramBotClient Client => _client ??= new TelegramBotClient(TOKEN);
        
        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);
            Console.ReadLine();
            cts.Cancel();
        }
    }
}