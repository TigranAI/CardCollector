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
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Controllers
{
    using static Logs;

    public class UpdateController : IUpdateHandler
    {
        private ConcurrentQueue<HandlerModel> Handlers;

        public UpdateController()
        {
            Handlers = new ConcurrentQueue<HandlerModel>();
            Task.Run(async () =>
            {
                while (true)
                {
                    if (Handlers.TryDequeue(out var handler)) await handler.PrepareAndExecute();
                    else Thread.Sleep(50);
                }
            });
        }

        async Task IUpdateHandler.HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            try
            {
                var handler = await BuildHandler(update);
                Handlers.Enqueue(handler);
            }
            catch (Exception e)
            {
                LogOutError(e);
            }
        }

        private async Task<HandlerModel> BuildHandler(Update update)
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

        public Task HandlePollingErrorAsync(ITelegramBotClient client, Exception e, CancellationToken ct)
        {
            switch (e)
            {
                case ApiRequestException:
                    LogOutWarning(e);
                    break;
                default:
                    LogOutError(e);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}