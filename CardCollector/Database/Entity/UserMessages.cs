using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Database.Entity
{
    public class UserMessages
    {
        public int MenuMessageId { get; set; } = -1;
        public int CollectIncomeMessageId { get; set; } = -1;
        public int TopUsersMessageId { get; set; } = -1;
        public int DailyTaskMessageId { get; set; } = -1;
        public int DailyTaskAlertMessageId { get; set; } = -1;
        public int DailyTaskProgressMessageId { get; set; } = -1;
        public HashSet<int> ChatMessages { get; set; } = new ();
        public HashSet<int> ChatStickers { get; set; } = new ();

        public async Task ClearChat(User user)
        {
            if (user.IsBlocked) return;
            await ClearMessages(user);
            await ClearStickers(user);
        }

        public async Task ClearMessages(User user)
        {
            if (user.IsBlocked) return;
            foreach (var messageId in ChatMessages)
            {
                await MessageController.DeleteMessage(user.ChatId, messageId);
            }

            ChatMessages.Clear();
        }

        public async Task ClearStickers(User user)
        {
            if (user.IsBlocked) return;
            foreach (var messageId in ChatStickers)
            {
                await MessageController.DeleteMessage(user.ChatId, messageId);
            }

            ChatStickers.Clear();
        }

        public async Task SendSticker(
            User user,
            string fileId,
            IReplyMarkup? keyboard = null)
        {
            if (user.IsBlocked) return;
            var messageId = await MessageController.SendSticker(user.ChatId, fileId, keyboard);
            if (messageId != -1) ChatStickers.Add(messageId);
        }

        public async Task EditMessage(
            User user,
            string message,
            InlineKeyboardMarkup? keyboard = null,
            ParseMode? parseMode = null)
        {
            if (user.IsBlocked) return;
            if (ChatMessages.Count == 0) await SendMessage(user, message, keyboard, parseMode);
            else
            {
                var messageId = await MessageController.EditMessage(user.ChatId, ChatMessages.Last(),
                    message, keyboard, parseMode);
                if (messageId != -1) ChatMessages.Add(messageId);
            }
        }

        public async Task SendMessage(
            User user,
            string message,
            IReplyMarkup? keyboard = null,
            ParseMode? parseMode = null)
        {
            if (user.IsBlocked) return;
            var messageId = await MessageController.SendMessage(user.ChatId, message, keyboard, parseMode);
            if (messageId != -1) ChatMessages.Add(messageId);
        }

        public async Task SendPhoto(
            User user,
            string fileId,
            string message,
            InlineKeyboardMarkup keyboard)
        {
            if (user.IsBlocked) return;
            var messageId = await MessageController.SendImage(user, fileId, message, keyboard);
            if (messageId != -1) ChatMessages.Add(messageId);
        }

        public async Task SendMenu(User user)
        {
            if (user.IsBlocked) return;
            await ClearChat(user);
            if (MenuMessageId != -1) await MessageController.DeleteMessage(user.ChatId, MenuMessageId);
            var isFirstOrderPicked = user.SpecialOrdersUser.Any(item => item.Id == 2);
            MenuMessageId = await MessageController.SendMessage(user.ChatId, Messages.main_menu,
                Keyboard.Menu(isFirstOrderPicked), ParseMode.Html);
        }

        public async Task SendDailyTaskProgress(User user, string message)
        {
            if (user.IsBlocked) return;
            if (DailyTaskProgressMessageId != -1)
                await MessageController.DeleteMessage(user.ChatId, DailyTaskProgressMessageId);
            DailyTaskProgressMessageId = await MessageController.SendMessage(user.ChatId, message);
            ChatMessages.Add(DailyTaskProgressMessageId);
        }

        public async Task SendDailyTaskAlert(User user)
        {
            if (user.IsBlocked) return;
            if (DailyTaskAlertMessageId != -1) await MessageController.DeleteMessage(user.ChatId, DailyTaskAlertMessageId);
            DailyTaskAlertMessageId = await MessageController.SendMessage(user.ChatId, Messages.daily_task_alertation);
            ChatMessages.Add(DailyTaskAlertMessageId);
        }

        public async Task SendDailyTaskComplete(User user)
        {
            if (user.IsBlocked) return;
            await ClearChat(user);
            if (DailyTaskMessageId != -1) await MessageController.DeleteMessage(user.ChatId, DailyTaskMessageId);
            DailyTaskMessageId = await MessageController.SendMessage(user.ChatId, Messages.pack_prize, Keyboard.MyPacks);
            ChatMessages.Add(DailyTaskMessageId);
        }

        public async Task SendTopUsers(User user, string message, InlineKeyboardMarkup keyboard)
        {
            if (user.IsBlocked) return;
            if (TopUsersMessageId != -1) await MessageController.DeleteMessage(user.ChatId, TopUsersMessageId);
            TopUsersMessageId = await MessageController.SendMessage(user.ChatId, message, keyboard, ParseMode.Html);
            ChatMessages.Add(TopUsersMessageId);
        }

        public async Task SendPiggyBankAlert(User user, string message)
        {
            if (user.IsBlocked) return;
            if (CollectIncomeMessageId != -1) await MessageController.DeleteMessage(user.ChatId, CollectIncomeMessageId);
            CollectIncomeMessageId = await MessageController.SendMessage(user.ChatId, message);
            ChatMessages.Add(CollectIncomeMessageId);
        }
    }
}