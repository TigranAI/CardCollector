using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Resources;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.DataBase.Entity
{
    public class UserMessages
    {
        public int MenuMessageId { get; set; } = -1;
        public int CollectIncomeMessageId { get; set; } = -1;
        public int TopUsersMessageId { get; set; } = -1;
        public int DailyTaskMessageId { get; set; } = -1;
        public int DailyTaskAlertMessageId { get; set; } = -1;
        public int DailyTaskProgressMessageId { get; set; } = -1;
        public ICollection<int> ChatMessages { get; set; } = new HashSet<int>();
        public ICollection<int> ChatStickers { get; set; } = new HashSet<int>();

        public async Task ClearChat(User user)
        {
            foreach (var messageId in ChatMessages)
            {
                await MessageController.DeleteMessage(user, messageId);
            }
            foreach (var messageId in ChatStickers)
            {
                await MessageController.DeleteMessage(user, messageId);
            }
            ChatMessages.Clear();
            ChatStickers.Clear();
        }

        public async Task ClearMessages(User user)
        {
            foreach (var messageId in ChatMessages)
            {
                await MessageController.DeleteMessage(user, messageId);
            }
            ChatMessages.Clear();
        }

        public async Task ClearStickers(User user)
        {
            foreach (var messageId in ChatStickers)
            {
                await MessageController.DeleteMessage(user, messageId);
            }
            ChatStickers.Clear();
        }

        public async Task SendSticker(
            User user,
            string fileId,
            IReplyMarkup? keyboard = null)
        {
            await ClearMessages(user);
            var messageId = await MessageController.SendSticker(user, fileId, keyboard);
            ChatStickers.Add(messageId);
        }

        public async Task EditMessage(
            User user,
            string message,
            InlineKeyboardMarkup? keyboard = null,
            ParseMode? parseMode = null)
        {
            var messageId = await MessageController.EditMessage(user, message, keyboard, parseMode);
            if (messageId != -1) ChatMessages.Add(messageId);
        }

        public async Task SendMessage(
            User user,
            string message,
            IReplyMarkup? keyboard = null,
            ParseMode? parseMode = null)
        {
            var messageId = await MessageController.SendMessage(user, message, keyboard, parseMode);
            if (messageId != -1) ChatMessages.Add(messageId);
        }

        public async Task SendMenu(User user)
        {
            await ClearChat(user);
            if (MenuMessageId != -1) await MessageController.DeleteMessage(user, MenuMessageId);
            MenuMessageId =
                await MessageController.SendMessage(user, Messages.main_menu, Keyboard.Menu, ParseMode.Html);
        }

        public async Task SendDailyTaskProgress(User user, string message)
        {
            if (DailyTaskProgressMessageId != -1) await MessageController.DeleteMessage(user, DailyTaskProgressMessageId);
            DailyTaskProgressMessageId = await MessageController.SendMessage(user, message);
        }

        public async Task SendDailyTaskAlert(User user)
        {
            if (DailyTaskAlertMessageId != -1) await MessageController.DeleteMessage(user, DailyTaskAlertMessageId);
            DailyTaskAlertMessageId = await MessageController.SendMessage(user, Messages.daily_task_alertation);
            ChatMessages.Add(DailyTaskAlertMessageId);
        }

        public async Task SendDailyTaskComplete(User user)
        {
            await ClearChat(user);
            if (DailyTaskMessageId != -1) await MessageController.DeleteMessage(user, DailyTaskMessageId);
            DailyTaskMessageId = await MessageController.SendMessage(user, Messages.pack_prize, Keyboard.MyPacks);
            ChatMessages.Add(DailyTaskMessageId);
        }

        public async Task SendTopUsers(User user, string message, InlineKeyboardMarkup keyboard)
        {
            if (TopUsersMessageId != -1) await MessageController.DeleteMessage(user, TopUsersMessageId);
            TopUsersMessageId = await MessageController.SendMessage(user, message, keyboard, ParseMode.Html);
        }
    }
}