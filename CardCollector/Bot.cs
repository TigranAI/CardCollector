using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.TimerTasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector
{
    using static MessageController;
    public static class Bot
    {
        private static TelegramBotClient? _client;
        public static TelegramBotClient Client => _client ??= new TelegramBotClient(AppSettings.TOKEN);

        private static readonly ManualResetEvent End = new(false);

        private static readonly IEnumerable<BotCommand> Commands = new[]
        {
            /*new BotCommand {Command = Text.start, Description = "Запуск бота"},*/
            new BotCommand {Command = MessageCommands.menu, Description = "Показать меню"},
            new BotCommand {Command = MessageCommands.help, Description = "Показать информацию"},
            /*new BotCommand {Command = "/error", Description = "Сообщить об ошибке"},*/
        };

        public static async Task Main(string[] args)
        {
            CheckArgs(args);
            await UpdateDatabase();
            
            TimerTask.SetupAll();
            
            var cts = new CancellationTokenSource();
            await Client.SetMyCommandsAsync(Commands, BotCommandScope.AllPrivateChats(), cancellationToken: cts.Token);
            Client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);
            
            Logs.LogOut("Bot started");
            End.WaitOne();
            await Client.CloseAsync();
            cts.Cancel();
            Logs.LogOut("Bot stopped");
        }

        public static async Task StopProgram()
        {
            await SessionController.CloseSessions();
            await ContextManager.DisposeAllAsync();
            End.Set();
        }

        private static void CheckArgs(string[] args)
        {
            foreach (var s in args)
            {
                var data = s.Split('=');
                switch (data[0])
                {
                    case "-debug":
                        Constants.DEBUG = bool.Parse(data[1]);
                        break;
                }
            }
        }

        private static async Task UpdateDatabase()
        {
            using (var context = new BotDatabaseContext())
            {
                await context.Database.MigrateAsync();
            }
        }
    }
}