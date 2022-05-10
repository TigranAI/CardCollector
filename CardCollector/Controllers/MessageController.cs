using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
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
        private static SortedList<DateTime, List<Task>> WaitQueue = new();

        public static void RunWaitQueueResolver()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (WaitQueue.Count == 0) Thread.Sleep(1000);
                    else
                    {
                        var item = WaitQueue.FirstOrDefault();
                        if (item.Key <= DateTime.Now)
                        {
                            foreach (var task in item.Value) await task;
                            WaitQueue.RemoveAt(0);
                        }
                        else Thread.Sleep((item.Key - DateTime.Now).Milliseconds);
                    }
                }
            });
        }

        private static void AddToWaitQueue(int seconds, Task task)
        {
            var key = DateTime.Now.AddSeconds(seconds);
            
            if (WaitQueue.TryGetValue(key, out var list)) list.Add(task);
            else WaitQueue.Add(key, new List<Task>() { task });
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
            catch (Exception e)
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
                    AddToWaitQueue(interval, SendMessage(chatId, message, keyboard, parseMode));
                else LogOutError(e);
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
                    AddToWaitQueue(interval, SendSticker(chatId, fileId, keyboard));
                else LogOutError(e);
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
                    AddToWaitQueue(interval, AnswerCallbackQuery(user, callbackQueryId, text, showAlert));
                else LogOutError(e);
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
                    AddToWaitQueue(interval, DeleteMessage(chatId, messageId));
                else LogOutError(e);
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
                    AddToWaitQueue(interval, AnswerInlineQuery(user, queryId, results, offset));
                else LogOutError(e);
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
            try
            {
                var result = await Bot.Client.SendInvoiceAsync(user.ChatId, title, description, payload,
                    AppSettings.PSB_TOKEN, currency.ToString(), prices, replyMarkup: keyboard,
                    disableNotification: true);
                user.Messages.ChatMessages.Add(result.MessageId);
                return result.MessageId;
            }
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                    AddToWaitQueue(interval, SendInvoice(user, title, description, payload, prices, keyboard, currency));
                else LogOutError(e);
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
                    AddToWaitQueue(interval, EditReplyMarkup(user, messageId, keyboard));
                else LogOutError(e);
            }
        }

        public static async Task<int> SendImage(User user, string fileId, string message, InlineKeyboardMarkup keyboard)
        {
            if (user.IsBlocked) return -1;
            return await SendImage(user.ChatId, fileId, message, keyboard);
        }

        public static async Task<int> SendImage(long chatId, string fileId, string message,
            InlineKeyboardMarkup? keyboard = null)
        {
            try
            {
                var msg = await Bot.Client.SendPhotoAsync(chatId, new InputOnlineFile(fileId), message,
                    protectContent: true, replyMarkup: keyboard, disableNotification: true);
                return msg.MessageId;
            }
            catch (ApiRequestException e)
            {
                if (e.ErrorCode == 429 && e.Parameters != null && e.Parameters.RetryAfter is { } interval)
                    AddToWaitQueue(interval, SendImage(chatId, fileId, message, keyboard));
                else LogOutError(e);
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
                    AddToWaitQueue(interval, SendDice(chatId, emoji));
                else LogOutError(e);
                return -1;
            }
        }
    }
}