using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Commands.MessageCommands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Controllers
{
    using static Logs;

    public static class MessageController
    {
        private static Dictionary<long, List<int>> _deletingMessagePool = new();

        public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            try
            {
                var executor = update.Type switch
                {
                    UpdateType.Message => await MessageCommand.Factory(update),
                    _ => throw new ArgumentOutOfRangeException()
                };
                var message = await executor.Execute();
                AddNewMessageToPool(message.Chat.Id, message.MessageId);
            }
            catch (ArgumentOutOfRangeException)
            {
                LogOut(update.Type.ToString());
            }
            catch (Exception e)
            {
                await HandleErrorAsync(client, e, ct);
            }
        }

        public static Task HandleErrorAsync(ITelegramBotClient client, Exception e, CancellationToken ct)
        {
            switch (e)
            {
                case ApiRequestException apiRequestException:
                    LogOutWarning($"API Error:[{apiRequestException.ErrorCode}] - {apiRequestException.Message}");
                    break;
                default:
                    LogOutError(e.ToString());
                    break;
            }

            return Task.CompletedTask;
        }

        public static async Task DeleteMessageAsync(long chatId, int messageId)
        {
            await Bot.Client.DeleteMessageAsync(chatId, messageId);
        }

        public static void AddNewMessageToPool(long chatId, int messageId)
        {
            try
            {
                _deletingMessagePool[chatId].Add(messageId);
            }
            catch (Exception)
            {
                _deletingMessagePool.Add(chatId, new List<int>());
                _deletingMessagePool[chatId].Add(messageId);
            }
        }

        public static async Task DeleteMessagesFromPool(long chatId)
        {
            try
            {
                foreach (var id in _deletingMessagePool[chatId])
                    await Bot.Client.DeleteMessageAsync(chatId, id);
                _deletingMessagePool[chatId].Clear();
                _deletingMessagePool.Remove(chatId);
            }
            catch (Exception)
            {
                /* ignore */
            }
        }

        public static async Task<Message> SendMessage(long chatId, string messageText, IReplyMarkup keyboard = null)
        {
            return await Bot.Client.SendTextMessageAsync(chatId, messageText, ParseMode.Html,
                replyMarkup: keyboard, disableNotification: true);
        }
    }
}