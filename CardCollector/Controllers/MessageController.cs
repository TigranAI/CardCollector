using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Commands;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Commands.InlineQueryHandler;
using CardCollector.Commands.MessageHandler;
using CardCollector.Commands.MyChatMemberHandler;
using CardCollector.Commands.PreCheckoutQueryHandler;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Controllers
{
    using static Logs;

    public static class MessageController
    {
        public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            try
            {
                var executor = update.Type switch
                {
                    UpdateType.Message => await MessageHandler.Factory(update),
                    UpdateType.CallbackQuery => await CallbackQueryHandler.Factory(update),
                    UpdateType.MyChatMember => await MyChatMemberCommand.Factory(update),
                    UpdateType.InlineQuery => await InlineQueryHandler.Factory(update),
                    UpdateType.ChosenInlineResult => await ChosenInlineResultHandler.Factory(update),
                    UpdateType.PreCheckoutQuery => await PreCheckoutQueryHandler.Factory(update),
                    _ => throw new ArgumentOutOfRangeException()
                };
                try
                {
                    await executor.PrepareAndExecute();
                }
                catch (Exception e)
                {
                    await SendError(update, e);
                }
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case ArgumentOutOfRangeException:
                        LogOut(update.Type);
                        break;
                    case ApiRequestException:
                        LogOutWarning(e.Message);
                        break;
                    default:
                        LogOutError(e);
                        break;
                }
            }
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
                await new SayError(user, context, exception).PrepareAndExecute();
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

        public static async Task<int> EditMessage(
            long chatId,
            int messageId,
            string message,
            InlineKeyboardMarkup? keyboard = null,
            ParseMode? parseMode = null)
        {
            try
            {
                var msg = await Bot.Client.EditMessageTextAsync(chatId, messageId, message, parseMode,
                    replyMarkup: keyboard);
                return msg.MessageId;
            }
            catch (Exception)
            {
                await DeleteMessage(chatId, messageId);
                return await SendMessage(chatId, message, keyboard, parseMode);
            }
        }

        public static async Task<int> SendMessage(
            long chatId,
            string message,
            IReplyMarkup? keyboard = null,
            ParseMode? parseMode = null)
        {
            try
            {
                var msg = await Bot.Client.SendTextMessageAsync(chatId, message, parseMode,
                    replyMarkup: keyboard, disableNotification: true);
                return msg.MessageId;
            }
            catch (Exception e)
            {
                LogOutWarning($"Cant send message: {e.Message}");
                return -1;
            }
        }

        public static async Task<int> SendSticker(
            long chatId,
            string fileId,
            IReplyMarkup? keyboard = null)
        {
            try
            {
                var msg = await Bot.Client.SendStickerAsync(chatId, fileId,
                    replyMarkup: keyboard, protectContent: true, disableNotification: true);
                return msg.MessageId;
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send sticker " + e.Message);
                return -1;
            }
        }


        public static async Task AnswerCallbackQuery(
            User user,
            string callbackQueryId,
            string text,
            bool showAlert = false)
        {
            if (user.IsBlocked) return;
            try
            {
                user.Session.PopLastCommand();
                await Bot.Client.AnswerCallbackQueryAsync(callbackQueryId, text, showAlert);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't answer CallbackQuery " + e.Message);
            }
        }

        public static async Task DeleteMessage(
            long chatId,
            int messageId)
        {
            try
            {
                await Bot.Client.DeleteMessageAsync(chatId, messageId);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't delete message " + e.Message);
            }
        }

        public static async Task AnswerInlineQuery(
            User user,
            string queryId,
            IEnumerable<InlineQueryResult> results,
            string offset)
        {
            if (user.IsBlocked) return;
            try
            {
                await Bot.Client.AnswerInlineQueryAsync(queryId, results, isPersonal: true, nextOffset: offset,
                    cacheTime: Constants.INLINE_RESULTS_CACHE_TIME);
            }
            catch (Exception e)
            {
                LogOutWarning($"Cant send answer inline query {e.Message}");
            }
        }

        public static async Task<int> SendInvoice(
            User user,
            string title,
            string description,
            string payload,
            IEnumerable<LabeledPrice> prices,
            InlineKeyboardMarkup? keyboard = null,
            Currency currency = Currency.RUB)
        {
            if (user.IsBlocked) return -1;
            var module = user.Session.GetModule<ShopModule>();
            var token = module.SelectedProvider switch
            {
                "Сбербанк" => AppSettings.SberbankToken,
                "ЮКасса" => AppSettings.YouKassaToken,
                _ => ""
            };

            try
            {
                var result = await Bot.Client.SendInvoiceAsync(user.ChatId, title, description, payload,
                    token, currency.ToString(), prices, replyMarkup: keyboard, disableNotification: true);
                user.Messages.ChatMessages.Add(result.MessageId);
                return result.MessageId;
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send invoice " + e.Message);
                return -1;
            }
        }

        public static async Task EditReplyMarkup(User user, int messageId, InlineKeyboardMarkup keyboard)
        {
            if (user.IsBlocked) return;
            try
            {
                await Bot.Client.EditMessageReplyMarkupAsync(user.ChatId, messageId, keyboard);
            }
            catch
            {
                /**/
            }
        }

        public static async Task<int> SendImage(User user, string fileId, string message, InlineKeyboardMarkup keyboard)
        {
            if (user.IsBlocked) return -1;
            try
            {
                var msg = await Bot.Client.SendPhotoAsync(user.ChatId, new InputOnlineFile(fileId), message,
                    replyMarkup: keyboard, disableNotification: true);
                return msg.MessageId;
            }
            catch (Exception e)
            {
                LogOutWarning($"Cant send image: {e.Message}");
                return -1;
            }
        }

        public static async Task<int> SendDice(long chatId, Emoji emoji)
        {
            try
            {
                var msg = await Bot.Client.SendDiceAsync(chatId, emoji, true);
                return msg.MessageId;
            }
            catch (Exception e)
            {
                LogOutWarning($"Cant send dice: {e.Message}");
                return -1;
            }
        }
    }
}