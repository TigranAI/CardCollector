using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Commands.InlineQueryHandler;
using CardCollector.Commands.MessageHandler;
using CardCollector.Commands.MyChatMember;
using CardCollector.Commands.PreCheckoutQueryHandler;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
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
                await executor.PrepareAndExecute();
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
            User user,
            string message,
            InlineKeyboardMarkup? keyboard = null,
            ParseMode? parseMode = null)
        {
            if (user.IsBlocked) return -1;
            if (user.Messages.ChatMessages.Count == 0) return await SendMessage(user, message, keyboard, parseMode);
            var messageId = user.Messages.ChatMessages.Last();
            return await EditMessage(user, messageId, message, keyboard, parseMode);
        }

        public static async Task<int> EditMessage(
            User user,
            int messageId,
            string message,
            InlineKeyboardMarkup? keyboard = null,
            ParseMode? parseMode = null)
        {
            if (user.IsBlocked) return -1;
            try
            {
                var msg = await Bot.Client.EditMessageTextAsync(user.ChatId, messageId, message, parseMode,
                    replyMarkup: keyboard);
                return msg.MessageId;
            }
            catch (Exception)
            {
                await DeleteMessage(user, messageId);
                return await SendMessage(user, message, keyboard, parseMode);
            }
        }

        public static async Task<int> SendMessage(
            User user,
            string message,
            IReplyMarkup? keyboard = null,
            ParseMode? parseMode = null)
        {
            if (user.IsBlocked) return -1;
            try
            {
                var msg = await Bot.Client.SendTextMessageAsync(user.ChatId, message, parseMode, 
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
            User user,
            string fileId,
            IReplyMarkup? keyboard = null)
        {
            if (user.IsBlocked) return -1;
            try
            {
                var msg = await Bot.Client.SendStickerAsync(user.ChatId, fileId, 
                    replyMarkup: keyboard, disableNotification: true);
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
                await Bot.Client.AnswerCallbackQueryAsync(callbackQueryId, text, showAlert);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't answer CallbackQuery " + e.Message);
            }
        }
        
        public static async Task DeleteMessage(
            User user,
            int messageId)
        {
            if (user.IsBlocked) return;
            user.Messages.ChatMessages.Remove(messageId);
            user.Messages.ChatStickers.Remove(messageId);
            try
            {
               await Bot.Client.DeleteMessageAsync(user.ChatId, messageId);
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
            string? offset = null)
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
                user.Session.Messages.Add(result.MessageId);
                return result.MessageId;
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send invoice " + e.Message);
                return -1;
            }
        }
    }
}