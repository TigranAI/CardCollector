using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Commands;
using CardCollector.Commands.MyChatMember;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Controllers
{
    using static Logs;

    /* Данный класс управляет получением обновлений и отправкой сообщений */
    public static class MessageController
    {
        /* Данный метод принимает обновления с сервера Телеграм, определяет для него обработчик и обрабатывет команду */
        public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            try
            {
                var executor = update.Type switch
                {
                    // Тип обновления - сообщение
                    UpdateType.Message => await MessageCommand.Factory(update),
                    // Тип обновления - нажатие на инлайн кнопку
                    UpdateType.CallbackQuery => await CallbackQueryCommand.Factory(update),
                    // Тип обновления - блокировка/добавление бота
                    UpdateType.MyChatMember => await MyChatMemberCommand.Factory(update),
                    // Тип обновления - вызов бота через @имя_бота
                    UpdateType.InlineQuery => await InlineQueryCommand.Factory(update),
                    // Тип обновления - выбор результата в инлайн меню
                    UpdateType.ChosenInlineResult => await ChosenInlineResultCommand.Factory(update),
                    // Тип обновления - платеж
                    UpdateType.PreCheckoutQuery => await PreCheckoutQueryCommand.Factory(update),
                    _ => throw new ArgumentOutOfRangeException()
                };
                // Обработать команду
                await executor.PrepareAndExecute();
            }
            catch (Exception e)
            {
                switch (e)
                {
                    // Случай, когда не определена обработка для данного типа обновленияот
                    case ArgumentOutOfRangeException:
                        LogOut(update.Type);
                        break;
                    // Ошибка, полученная со стороны Telegram API
                    case ApiRequestException:
                        LogOutWarning(e.Message);
                        break;
                    // Прочие ошибки
                    default:
                        LogOutError(e);
                        break;
                }
            }
        }

        // Обработка ошибок, полученных от сервера Телеграм
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

        /* Метод для отправки сообщения
         user - пользователь, которому необходимо отправить сообщение
         message - текст сообщения
         keyboard - клавиатура, которую надо добавить к сообщению */
        public static async Task EditMessage(UserEntity user, string message, IReplyMarkup keyboard = null,
            ParseMode? parseMode = null)
        {
            if (!user.IsBlocked)
                try
                {
                    if (user.Session.Messages.Count > 0 && keyboard is InlineKeyboardMarkup or null)
                        await Bot.Client.EditMessageTextAsync(user.ChatId, user.Session.Messages.Last(),
                            message, parseMode, replyMarkup: keyboard != null ? (InlineKeyboardMarkup) keyboard : null);
                    else await SendMessage(user, message, keyboard, parseMode);
                }
                catch
                {
                    await user.ClearChat();
                    await SendMessage(user, message, keyboard, parseMode);
                }
        }

        public static async Task<int> DeleteAndSend(UserEntity user, int prevMsgId, string message,
            IReplyMarkup keyboard = null, ParseMode? parseMode = null)
        {
            if (!user.IsBlocked)
            {
                if (prevMsgId != -1)
                    try
                    {
                        await Bot.Client.DeleteMessageAsync(user.ChatId, prevMsgId);
                    }
                    catch
                    {
                        /**/
                    }

                try
                {
                    var result = await Bot.Client.SendTextMessageAsync(user.ChatId, message, parseMode,
                        replyMarkup: keyboard, disableNotification: true);
                    return result.MessageId;
                }
                catch
                {
                    /**/
                }
            }

            return -1;
        }

        public static async Task SendMessage(UserEntity user, string message, IReplyMarkup keyboard = null,
            ParseMode? parseMode = null, bool addToList = true)
        {
            if (!user.IsBlocked)
                try
                {
                    var result = await Bot.Client.SendTextMessageAsync(user.ChatId, message, parseMode,
                        replyMarkup: keyboard, disableNotification: true);
                    if (addToList)
                        user.Session.Messages.Add(result.MessageId);
                }
                catch (Exception e)
                {
                    LogOut(e);
                }
        }

        /* Метод для отправки стикера
         user - пользователь, которому необходимо отправить сообщение
         fileId - id стикера, расположенного на серверах телеграм */
        public static async Task<Message> SendSticker(UserEntity user, string fileId, IReplyMarkup keyboard = null)
        {
            if (!user.IsBlocked)
                try
                {
                    await user.ClearChat();
                    var result = await Bot.Client.SendStickerAsync(user.ChatId, fileId, true, replyMarkup: keyboard);
                    user.Session.StickerMessages.Add(result.MessageId);
                    return result;
                }
                catch (Exception e)
                {
                    LogOutWarning("Can't send sticker " + e.Message);
                }

            return new Message();
        }

        /* Метод для редактирования клавиатуры под сообщением
         user - пользователь, которому необходимо отредактировать сообщение
         messageId - Id сообщения
         keyboard - новая клавиатура, которую надо добавить к сообщению */
        public static async Task<Message> EditReplyMarkup(UserEntity user, InlineKeyboardMarkup keyboard,
            int messageId = -1)
        {
            if (!user.IsBlocked)
                try
                {
                    var msgId = messageId != -1 ? messageId : user.Session.Messages.Last();
                    return await Bot.Client.EditMessageReplyMarkupAsync(user.ChatId, msgId, keyboard);
                }
                catch (Exception e)
                {
                    LogOutWarning("Can't edit reply markup " + e.Message);
                }

            return new Message();
        }

        public static async Task AnswerCallbackQuery(UserEntity user, string callbackQueryId, string text,
            bool showAlert = false)
        {
            try
            {
                if (!user.IsBlocked)
                    await Bot.Client.AnswerCallbackQueryAsync(callbackQueryId, text, showAlert);
                user.Session.UndoCurrentCommand();
            }
            catch (Exception e)
            {
                LogOutWarning("Can't answer callbackquery " + e.Message);
            }
        }

        /* Метод для удаления сообщения
         user - пользователь, которому необходимо удалить сообщение
         messageId - Id сообщения */
        public static async Task DeleteMessage(UserEntity user, int messageId, bool sticker = false)
        {
            try
            {
                if (sticker) user.Session.StickerMessages.Remove(messageId);
                else user.Session.Messages.Remove(messageId);
                if (!user.IsBlocked)
                    await Bot.Client.DeleteMessageAsync(user.ChatId, messageId);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't delete message " + e.Message);
            }
        }

        /* Метод для ответа на запрос @имя_бота
         queryId - Id запроса
         results - массив объектов InlineQueryResult */
        public static async Task AnswerInlineQuery(string queryId, IEnumerable<InlineQueryResult> results,
            string offset = null)
        {
            await Bot.Client.AnswerInlineQueryAsync(queryId, results, isPersonal: true, nextOffset: offset,
                cacheTime: Constants.INLINE_RESULTS_CACHE_TIME);
        }

        public static async Task<Message> SendInvoice(UserEntity user, string title, string description,
            string payload, IEnumerable<LabeledPrice> prices, InlineKeyboardMarkup keyboard = null,
            Currency currency = Currency.USD)
        {
            if (!user.IsBlocked)
                try
                {
                    await user.ClearChat();
                    var result = await Bot.Client.SendInvoiceAsync(user.ChatId, title, description, payload,
                        AppSettings.PAYMENT_PROVIDER, currency.ToString(), prices, replyMarkup: keyboard,
                        disableNotification: true);
                    user.Session.Messages.Add(result.MessageId);
                    return result;
                }
                catch (Exception e)
                {
                    LogOutWarning("Can't send photo " + e.Message);
                }

            return new Message();
        }
    }
}