using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.TimerTasks;
using Telegram.Bot;
using Telegram.Bot.Types;
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
            new BotCommand {Command = "/menu", Description = "Показать меню"},/*
            new BotCommand {Command = "/help", Description = "Показать информацию"},
            new BotCommand {Command = "/error", Description = "Сообщить об ошибке"},*/
        };

        public static void Main(string[] args)
        {
            Logs.LogOut("Bot started");
            
            _timer.Elapsed += SavingChanges;
            _timer.Elapsed += UserDao.ClearMemory;
            TimerTask.SetupAll();
            
            var cts = new CancellationTokenSource();
            Client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);
            Client.SetMyCommandsAsync(_commands, BotCommandScope.AllPrivateChats(), cancellationToken: cts.Token);

            _end.WaitOne();
            Logs.LogOut("Stopping program");
            
            cts.Cancel();
        }

        public static async Task StopProgram()
        {
            await BotDatabase.SaveData();
            await UserDao.ClearMemory();
            _end.Set();
        }

        private static async void SavingChanges(object o, ElapsedEventArgs e)
        {
            try {
                await BotDatabase.SaveData();
            } catch (Exception) { /**/ }
        }
    }
}