using System;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Commands;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Commands.InlineQueryHandler;
using CardCollector.Commands.MessageHandler;
using CardCollector.Commands.MyChatMemberHandler;
using CardCollector.Commands.PreCheckoutQueryHandler;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Controllers
{
    using static Logs;

    public static class UpdateController
    {
        public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            LogOut(Utilities.ToJson(update, Formatting.Indented));
            try
            {
                var handler = await BuildHandler(update);
                await handler.PrepareAndExecute();
            }
            catch (Exception e)
            {
                await SendError(update, e);
            }
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

        private static async Task SendError(Update update, Exception exception)
        {
            try
            {
                var context = new BotDatabaseContext();
                var userDetails = update.Type switch
                {
                    UpdateType.Message => update.Message!.From,
                    UpdateType.CallbackQuery => update.CallbackQuery!.From,
                    UpdateType.ChosenInlineResult => update.ChosenInlineResult!.From,
                    UpdateType.MyChatMember => update.MyChatMember!.From,
                    UpdateType.InlineQuery => update.InlineQuery!.From,
                    UpdateType.PreCheckoutQuery => update.PreCheckoutQuery!.From,
                    _ => null
                };
                if (userDetails == null) return;
                var user = await context.Users.FindUser(userDetails);
                if (user.IsBlocked) return;
                
                await new SayError()
                    .SetException(exception)
                    .Init(user, context, update)
                    .PrepareAndExecute();
            }
            catch (Exception e)
            {
                LogOutError(exception);
                LogOutError(e);
            }
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