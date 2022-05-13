using System.Threading;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.TimerTasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector
{
    using static UpdateController;
    public static class Bot
    {
        private static TelegramBotClient? _client;
        public static TelegramBotClient Client => _client ??= new TelegramBotClient(AppSettings.TOKEN);

        private static readonly ManualResetEvent End = new(false);

        public static async Task Main(string[] args)
        {
            CheckArgs(args);
            await UpdateDatabase();

            TimerTask.SetupAll();
            
            await Client.SetMyCommandsAsync(Constants.PrivateCommands, BotCommandScope.AllPrivateChats());
            await Client.SetMyCommandsAsync(Constants.GroupCommands, BotCommandScope.AllGroupChats());

            Client.StartReceiving(HandleUpdateAsync, HandleErrorAsync);
            UpdateController.Run();
            
            MessageController.RunWaitQueueResolver();
            
            Logs.LogOut("Bot started");
            End.WaitOne();
            await Client.CloseAsync();
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