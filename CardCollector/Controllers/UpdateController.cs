using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Commands;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Commands.InlineQueryHandler;
using CardCollector.Commands.MessageHandler;
using CardCollector.Commands.MyChatMemberHandler;
using CardCollector.Commands.PreCheckoutQueryHandler;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Controllers
{
    using static Logs;

    public static class UpdateController
    {
        private static ConcurrentQueue<HandlerModel> Handlers = new();

        public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            var handler = await BuildHandler(update);
            Handlers.Enqueue(handler);
        }

        public static void Run()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (Handlers.TryDequeue(out var handler)) await handler.PrepareAndExecute();
                    else Thread.Sleep(50);
                }
            });
        }

        private static async Task<HandlerModel> BuildHandler(Update update)
        {
            return update.Type switch
            {
                UpdateType.Message => await MessageHandler.Factory(update),
                UpdateType.CallbackQuery => await CallbackQueryHandler.Factory(update),
                UpdateType.MyChatMember => await MyChatMemberHandler.Factory(update),
                UpdateType.InlineQuery => await InlineQueryHandler.Factory(update),
                UpdateType.ChosenInlineResult => await ChosenInlineResultHandler.Factory(update),
                UpdateType.PreCheckoutQuery => await PreCheckoutQueryHandler.Factory(update),
                _ => new IgnoreHandler()
            };
        }

        public static Task HandleErrorAsync(ITelegramBotClient client, Exception e, CancellationToken ct)
        {
            switch (e)
            {
                case ApiRequestException:
                    LogOutWarning(e.Message);
                    break;
                default:
                    LogOutError(e);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}