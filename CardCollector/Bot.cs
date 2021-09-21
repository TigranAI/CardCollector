using System;
using System.Collections.Generic;
using System.Timers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using CancellationTokenSource = System.Threading.CancellationTokenSource;

namespace CardCollector
{
    using static Controllers.MessageController;
    public static class Bot
    {
        private static TelegramBotClient _client;
        public static TelegramBotClient Client => _client ??= new TelegramBotClient(AppSettings.TOKEN);

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
            Client.SetMyCommandsAsync(_commands, BotCommandScope.AllPrivateChats());
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
            timer.Elapsed += UserDao.ClearMemory;
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