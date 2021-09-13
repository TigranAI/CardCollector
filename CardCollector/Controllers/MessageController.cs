using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Message = CardCollector.Commands.Message.Message;
using CallBackQuery = CardCollector.Commands.CallbackQuery.CallbackQuery;
using MyChatMember = CardCollector.Commands.MyChatMember.MyChatMember;
using InlineQuery = CardCollector.Commands.InlineQuery.InlineQuery;
using ChosenInlineResult = CardCollector.Commands.ChosenInlineResult.ChosenInlineResult;
using TgMessage = Telegram.Bot.Types.Message;

namespace CardCollector.Controllers
{
    using static Logs;

    public static class MessageController
    {
        private static readonly Dictionary<long, List<int>> DeletingMessagePool = new();

        public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            try
            {
                var executor = update.Type switch
                {
                    UpdateType.Message => await Message.Factory(update),
                    UpdateType.CallbackQuery => await CallBackQuery.Factory(update),
                    UpdateType.MyChatMember => await MyChatMember.Factory(update),
                    UpdateType.InlineQuery => await InlineQuery.Factory(update),
                    UpdateType.ChosenInlineResult => await ChosenInlineResult.Factory(update),
                    _ => throw new ArgumentOutOfRangeException()
                };
                // var message =
                await executor.Execute();
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

        public static void AddNewMessageToPool(UserEntity user, int messageId)
        {
            try
            {
                DeletingMessagePool[user.ChatId].Add(messageId);
            }
            catch (Exception)
            {
                DeletingMessagePool.Add(user.ChatId, new List<int>());
                DeletingMessagePool[user.ChatId].Add(messageId);
            }
        }

        public static async Task DeleteMessagesFromPool(UserEntity user)
        {
            try
            {
                foreach (var id in DeletingMessagePool[user.ChatId])
                    await DeleteMessage(user, id);
                DeletingMessagePool[user.ChatId].Clear();
                DeletingMessagePool.Remove(user.ChatId);
            }
            catch (Exception)
            {
                /* ignore */
            }
        }

        public static async Task<TgMessage> SendMessage(UserEntity user, string message, IReplyMarkup keyboard = null)
        {
            try
            {
                if (!user.IsBlocked)
                    return await Bot.Client.SendTextMessageAsync(user.ChatId, message, replyMarkup: keyboard, disableNotification: true);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send text message " + e);
            }
            return new TgMessage();
        }
        
        public static async Task<TgMessage> SendTextWithHtml(UserEntity info, string message, IReplyMarkup keyboard = null)
        {
            try
            {
                if (!info.IsBlocked)
                    return await Bot.Client.SendTextMessageAsync(info.ChatId, message, ParseMode.Html, replyMarkup: keyboard, disableNotification: true);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send text message with html " + e);
            }
            return new TgMessage();
        }
        
        public static async Task<TgMessage> SendSticker(UserEntity info, string fileId)
        {
            try
            {
                if (!info.IsBlocked)
                    return await Bot.Client.SendStickerAsync(info.ChatId, fileId, true);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send sticker " + e);
            }
            return new TgMessage();
        }

        public static async Task<TgMessage> EditMessage(UserEntity info, int messageId, string message, InlineKeyboardMarkup keyboard = null)
        {
            try
            {
                if (!info.IsBlocked)
                    return await Bot.Client.EditMessageTextAsync(info.ChatId, messageId, message, replyMarkup: keyboard);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't edit message text " + e);
            }
            return new TgMessage();
        }

        public static async Task<TgMessage> EditReplyMarkup(UserEntity info, int messageId, InlineKeyboardMarkup keyboard)
        {
            try
            {
                if (!info.IsBlocked)
                    return await Bot.Client.EditMessageReplyMarkupAsync(info.ChatId, messageId, keyboard);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't edit reply markup " + e);
            }
            return new TgMessage();
        }
        
        public static async Task DeleteMessage(UserEntity user, int messageId)
        {
            try
            {
                if (!user.IsBlocked)
                    await Bot.Client.DeleteMessageAsync(user.ChatId, messageId);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't delete message " + e);
            }
        }

        public static async Task<TgMessage> SendImage(UserEntity info, InputOnlineFile inputOnlineFile, string message = null, InlineKeyboardMarkup replyMarkup = null)
        {
            try
            {
                if (!info.IsBlocked)
                    return await Bot.Client.SendPhotoAsync(info.ChatId, inputOnlineFile, message, replyMarkup: replyMarkup, disableNotification: true);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send photo " + e);
            }
            return new TgMessage();
        }

        public static async Task AnswerInlineQuery(string queryId, IEnumerable<InlineQueryResult> results, string offset = null)
        {
            await Bot.Client.AnswerInlineQueryAsync(queryId, results, isPersonal: true, nextOffset: offset, cacheTime: Constants.INLINE_RESULTS_CACHE_TIME);
        }
    }
}