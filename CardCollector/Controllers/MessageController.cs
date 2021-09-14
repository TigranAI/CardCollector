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
                    UpdateType.Message => await Message.Factory(update),
                    // Тип обновления - нажатие на инлайн кнопку
                    UpdateType.CallbackQuery => await CallBackQuery.Factory(update),
                    // Тип обновления - блокировка/добавление бота
                    UpdateType.MyChatMember => await MyChatMember.Factory(update),
                    // Тип обновления - вызов бота через @имя_бота
                    UpdateType.InlineQuery => await InlineQuery.Factory(update),
                    // Тип обновления - выбор результата в инлайн меню
                    UpdateType.ChosenInlineResult => await ChosenInlineResult.Factory(update),
                    _ => throw new ArgumentOutOfRangeException()
                };
                // Обработать команду
                await executor.Execute();
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
        
        /* Метод для отправки сообщения с разметкой html
         user - пользователь, которому необходимо отправить сообщение
         message - текст сообщения
         keyboard - клавиатура, которую надо добавить к сообщению */
        public static async Task<TgMessage> SendTextWithHtml(UserEntity user, string message, IReplyMarkup keyboard = null)
        {
            try
            {
                if (!user.IsBlocked)
                    return await Bot.Client.SendTextMessageAsync(user.ChatId, message, ParseMode.Html, replyMarkup: keyboard, disableNotification: true);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send text message with html " + e);
            }
            return new TgMessage();
        }
        
        /* Метод для отправки стикера
         user - пользователь, которому необходимо отправить сообщение
         fileId - id стикера, расположенного на серверах телеграм */
        public static async Task<TgMessage> SendSticker(UserEntity user, string fileId)
        {
            try
            {
                if (!user.IsBlocked)
                    return await Bot.Client.SendStickerAsync(user.ChatId, fileId, true);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send sticker " + e);
            }
            return new TgMessage();
        }
        
        /* Метод для редактирования сообщения
         user - пользователь, которому необходимо отредактировать сообщение
         messageId - id сообщения
         message - текст сообщения
         keyboard - клавиатура, которую надо добавить к сообщению */
        public static async Task<TgMessage> EditMessage(UserEntity user, int messageId, string message, InlineKeyboardMarkup keyboard = null)
        {
            try
            {
                if (!user.IsBlocked)
                    return await Bot.Client.EditMessageTextAsync(user.ChatId, messageId, message, replyMarkup: keyboard);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't edit message text " + e);
            }
            return new TgMessage();
        }

        /* Метод для редактирования клавиатуры под сообщением
         user - пользователь, которому необходимо отредактировать сообщение
         messageId - Id сообщения
         keyboard - новая клавиатура, которую надо добавить к сообщению */
        public static async Task<TgMessage> EditReplyMarkup(UserEntity user, int messageId, InlineKeyboardMarkup keyboard)
        {
            try
            {
                if (!user.IsBlocked)
                    return await Bot.Client.EditMessageReplyMarkupAsync(user.ChatId, messageId, keyboard);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't edit reply markup " + e);
            }
            return new TgMessage();
        }
        
        /* Метод для удаления сообщения
         user - пользователь, которому необходимо удалить сообщение
         messageId - Id сообщения */
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

        /* Метод для отправки изображения
         user - пользователь, которому необходимо отправить сообщение
         inputOnlineFile - фото, которое необходимо отправить
         message - текст сообщения
         keyboard - клавиатура, которую надо добавить к сообщению */
        public static async Task<TgMessage> SendImage(UserEntity user, InputOnlineFile inputOnlineFile, string message = null, InlineKeyboardMarkup keyboard = null)
        {
            try
            {
                if (!user.IsBlocked)
                    return await Bot.Client.SendPhotoAsync(user.ChatId, inputOnlineFile, message, replyMarkup: keyboard, disableNotification: true);
            }
            catch (Exception e)
            {
                LogOutWarning("Can't send photo " + e);
            }
            return new TgMessage();
        }

        /* Метод для ответа на запрос @имя_бота
         queryId - Id запроса
         results - массив объектов InlineQueryResult */
        public static async Task AnswerInlineQuery(string queryId, IEnumerable<InlineQueryResult> results, string offset = null)
        {
            await Bot.Client.AnswerInlineQueryAsync(queryId, results, isPersonal: true, nextOffset: offset, cacheTime: Constants.INLINE_RESULTS_CACHE_TIME);
        }
    }
}