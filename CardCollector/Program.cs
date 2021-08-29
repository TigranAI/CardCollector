using System;
using System.Threading;
using Telegram.Bot;

namespace CardCollector
{
    using static Controllers.MessageController;
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Bot.Client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);
            Console.ReadLine();
            cts.Cancel();
        }
    }
}