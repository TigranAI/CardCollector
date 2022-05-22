using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database.Entity;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Others
{
    public static class TelegramChatExtensions
    {
        public static async Task<int> SendMessage(this TelegramChat chat, string message, IReplyMarkup? keyboard = null,
            ParseMode parseMode = ParseMode.Html)
        {
            if (chat.IsBlocked) return -1;
            return await MessageController.SendMessage(chat.ChatId, message, keyboard, parseMode);
        }

        /*public static async Task<int> EditMessage(this TelegramChat chat, string message,
            InlineKeyboardMarkup? keyboard = null, ParseMode? parseMode = null)
        {
            if (chat.IsBlocked) return -1;
            if (chat.ChatMessages.Count == 0) return await SendMessage(chat, message, keyboard, parseMode);
            return await EditMessage(chat, message,chat.ChatMessages.Last(), keyboard, parseMode);
        }*/

        public static async Task<int> EditMessage(this TelegramChat chat, string message, int messageId,
            InlineKeyboardMarkup? keyboard = null, ParseMode parseMode = ParseMode.Html)
        {
            if (chat.IsBlocked) return -1;
            return await MessageController.EditMessage(chat.ChatId, messageId, message, keyboard, parseMode);
        }

        public static async Task<int> SendSticker(this TelegramChat chat, string fileId, IReplyMarkup? keyboard = null)
        {
            if (chat.IsBlocked) return -1;
            return await MessageController.SendSticker(chat.ChatId, fileId, keyboard);
        }

        public static async Task DeleteMessage(this TelegramChat chat, int messageId)
        {
            if (chat.IsBlocked) return;
            await MessageController.DeleteMessage(chat.ChatId, messageId);
        }

        public static async Task<int> SendDice(this TelegramChat chat, Emoji emoji)
        {
            if (chat.IsBlocked) return -1;
            return await MessageController.SendDice(chat.ChatId, emoji);
        }
    }
}