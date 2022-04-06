using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Controllers
{
    using static Logs;

    public static class MessageController
    {
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
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                {
                    Thread.Sleep(interval);
                    return await SendMessage(chatId, message, keyboard, parseMode);
                }

                LogOutWarning($"Cant send message: {e.Message}");
                LogOutError(e);
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
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                {
                    Thread.Sleep(interval);
                    return await SendSticker(chatId, fileId, keyboard);
                }
                LogOutWarning("Can't send sticker " + e.Message);
                LogOutError(e);
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
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                {
                    Thread.Sleep(interval);
                    await AnswerCallbackQuery(user, callbackQueryId, text, showAlert);
                    return;
                }
                LogOutWarning("Can't answer CallbackQuery " + e.Message);
                LogOutError(e);
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
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                {
                    Thread.Sleep(interval);
                    await DeleteMessage(chatId, messageId);
                    return;
                }
                LogOutWarning("Can't delete message " + e.Message);
                LogOutError(e);
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
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                {
                    Thread.Sleep(interval);
                    await AnswerInlineQuery(user, queryId, results, offset);
                    return;
                }
                LogOutWarning($"Cant send answer inline query {e.Message}");
                LogOutError(e);
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
            /*var token = module.SelectedProvider switch
            {
                "Сбербанк" => AppSettings.SberbankToken,
                "ЮКасса" => AppSettings.YouKassaToken,
                "ПСБ" => AppSettings.PSBToken,
                _ => ""
            };*/

            try
            {
                var result = await Bot.Client.SendInvoiceAsync(user.ChatId, title, description, payload,
                    AppSettings.PSBToken, currency.ToString(), prices, replyMarkup: keyboard, disableNotification: true);
                user.Messages.ChatMessages.Add(result.MessageId);
                return result.MessageId;
            }
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                {
                    Thread.Sleep(interval);
                    return await SendInvoice(user, title, description, payload, prices, keyboard, currency);
                }
                LogOutWarning("Can't send invoice " + e.Message);
                LogOutError(e);
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
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                {
                    Thread.Sleep(interval);
                    await EditReplyMarkup(user, messageId, keyboard);
                }
            }
        }

        public static async Task<int> SendImage(User user, string fileId, string message, InlineKeyboardMarkup keyboard)
        {
            if (user.IsBlocked) return -1;
            try
            {
                var msg = await Bot.Client.SendPhotoAsync(user.ChatId, new InputOnlineFile(fileId), message,
                    protectContent: true, replyMarkup: keyboard, disableNotification: true);
                return msg.MessageId;
            }
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                {
                    Thread.Sleep(interval);
                    return await SendImage(user, fileId, message, keyboard);
                }
                LogOutWarning($"Cant send image: {e.Message}");
                LogOutError(e);
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
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                {
                    Thread.Sleep(interval);
                    return await SendDice(chatId, emoji);
                }
                LogOutWarning($"Cant send dice: {e.Message}");
                LogOutError(e);
                return -1;
            }
        }
    }
}