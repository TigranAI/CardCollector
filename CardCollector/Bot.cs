using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Commands;
using CardCollector.Commands.CallbackQuery;
using CardCollector.Commands.ChosenInlineResult;
using CardCollector.Commands.Message;
using CardCollector.DailyTasks.CustomTasks;
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

        private static readonly ManualResetEvent End = new(false);
        private static readonly Timer Timer = new () {
            AutoReset = true,
            Enabled = true,
            Interval = Constants.SAVING_CHANGES_INTERVAL
        };

        private static readonly IEnumerable<BotCommand> Commands = new[]
        {
            /*new BotCommand {Command = Text.start, Description = "Запуск бота"},*/
            new BotCommand {Command = Text.menu, Description = "Показать меню"},
            new BotCommand {Command = Text.help, Description = "Показать информацию"},
            /*new BotCommand {Command = "/error", Description = "Сообщить об ошибке"},*/
        };

        public static async Task Main(string[] args)
        {
            checkArgs(args);
            initCommands();
            Logs.LogOut("Bot started");
            
            Timer.Elapsed += SavingChanges;
            Timer.Elapsed += UserDao.ClearMemory;
            TimerTask.SetupAll();
            
            var cts = new CancellationTokenSource();
            await Client.SetMyCommandsAsync(Commands, BotCommandScope.AllPrivateChats(), cancellationToken: cts.Token);
            Client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);

            End.WaitOne();
            Logs.LogOut("Stopping program");
            
            cts.Cancel();
        }

        private static void initCommands()
        {
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetTypes())
            {
                if (Attribute.IsDefined(type, typeof(Attributes.Abstract))) continue;
                if (Attribute.IsDefined(type, typeof(Attributes.CallbackQuery)))
                    CallbackQueryHandler.Commands.Add(type);
                else if (Attribute.IsDefined(type, typeof(Attributes.ChosenInlineResult)))
                    ChosenInlineResultHandler.Commands.Add(type);
                else if (Attribute.IsDefined(type, typeof(Attributes.InlineQuery)))
                    InlineQueryHandler.Commands.Add(type);
                else if (Attribute.IsDefined(type, typeof(Attributes.Message)))
                    MessageHandler.Commands.Add(type);
                else if (Attribute.IsDefined(type, typeof(Attributes.PreCheckoutQuery)))
                    PreCheckoutQueryHandler.Commands.Add(type);
            }
        }

        private static void checkArgs(string[] args)
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

        public static async Task StopProgram()
        {
            SendStickers.WriteLogs();
            CollectIncome.WriteLogs();
            SendStickerHandler.WriteLogs();
            BuyGemsItem.WriteLogs();
            ConfirmSelling.WriteLogs();
            await BotDatabase.SaveData();
            await UserDao.ClearMemory();
            End.Set();
        }

        private static async void SavingChanges(object o, ElapsedEventArgs e)
        {
            try {
                await BotDatabase.SaveData();
            } catch (Exception) { /**/ }
        }
    }
}