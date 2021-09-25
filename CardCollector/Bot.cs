using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using CancellationTokenSource = System.Threading.CancellationTokenSource;
using Timer = System.Timers.Timer;

namespace CardCollector
{
    using static Controllers.MessageController;
    public static class Bot
    {
        private static TelegramBotClient _client;
        public static TelegramBotClient Client => _client ??= new TelegramBotClient(AppSettings.TOKEN);

        private static readonly ManualResetEvent _end = new(false);
        private static readonly Timer _timer = new () {
            AutoReset = true,
            Enabled = true,
            Interval = Constants.SAVING_CHANGES_INTERVAL
        };

        private static readonly IEnumerable<BotCommand> _commands = new[]
        {
            new BotCommand {Command = "/menu", Description = "Показать меню"},
            new BotCommand {Command = "/help", Description = "Показать информацию"},
            new BotCommand {Command = "/error", Description = "Сообщить об ошибке"},
        };

        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);
            Client.SetMyCommandsAsync(_commands, BotCommandScope.AllPrivateChats(), cancellationToken: cts.Token);
            
            _timer.Elapsed += SavingChanges;
            _timer.Elapsed += UserDao.ClearMemory;
            
            _end.WaitOne();
            Logs.LogOut("Stopping program");
            
            cts.Cancel();
        }

        public static void StopProgram()
        {
            _timer.Elapsed += (_, _) =>
            {
                _timer.Stop();
                UserDao.EndOfProgram();
                _end.Set();
            };
        }

        private static void SavingChanges(object o, ElapsedEventArgs e)
        {
            try {
                CardCollectorDatabase.SaveAllChangesAsync();
            } catch (Exception) { /*ignored*/ }
        }
    }
}