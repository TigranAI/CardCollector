using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Database.Entity
{
    public class UserMessages
    {
        [Key, ForeignKey("id")]
        public virtual User User { get; set; }
        public int MenuMessageId { get; set; } = -1;
        public int CollectIncomeMessageId { get; set; } = -1;
        public int TopUsersMessageId { get; set; } = -1;
        public int DailyTaskMessageId { get; set; } = -1;
        public int DailyTaskAlertMessageId { get; set; } = -1;
        public int DailyTaskProgressMessageId { get; set; } = -1;
        public HashSet<int> ChatMessages { get; set; } = new ();
        public HashSet<int> ChatStickers { get; set; } = new ();

        public async Task ClearChat()
        {
            if (User.IsBlocked) return;
            await ClearMessages();
            await ClearStickers();
        }

        public async Task ClearMessages()
        {
            if (User.IsBlocked) return;
            foreach (var messageId in ChatMessages)
            {
                await DeleteMessage(User.ChatId, messageId);
            }

            ChatMessages.Clear();
        }

        public async Task ClearStickers()
        {
            if (User.IsBlocked) return;
            foreach (var messageId in ChatStickers)
            {
                await DeleteMessage(User.ChatId, messageId);
            }

            ChatStickers.Clear();
        }

        public async Task SendSticker(string fileId,
            IReplyMarkup? keyboard = null)
        {
            if (User.IsBlocked) return;
            var messageId = await MessageController.SendSticker(User.ChatId, fileId, keyboard);
            if (messageId != -1) ChatStickers.Add(messageId);
        }

        public async Task EditMessage(string message,
            InlineKeyboardMarkup? keyboard = null,
            ParseMode parseMode = ParseMode.Html)
        {
            if (User.IsBlocked) return;
            if (ChatMessages.Count == 0) await SendMessage(message, keyboard, parseMode);
            else
            {
                var messageId = await MessageController.EditMessage(User.ChatId, ChatMessages.Last(),
                    message, keyboard, parseMode);
                if (messageId != -1) ChatMessages.Add(messageId);
            }
        }

        public async Task SendMessage(string message,
            IReplyMarkup? keyboard = null,
            ParseMode parseMode = ParseMode.Html)
        {
            if (User.IsBlocked) return;
            var messageId = await MessageController.SendMessage(User.ChatId, message, keyboard, parseMode);
            if (messageId != -1) ChatMessages.Add(messageId);
        }

        public async Task SendPhoto(string fileId,
            string message,
            InlineKeyboardMarkup keyboard)
        {
            if (User.IsBlocked) return;
            var messageId = await MessageController.SendImage(User, fileId, message, keyboard);
            if (messageId != -1) ChatMessages.Add(messageId);
        }

        public async Task SendMenu()
        {
            if (User.IsBlocked) return;
            await ClearChat();
            if (MenuMessageId != -1) await DeleteMessage(User.ChatId, MenuMessageId);
            var isFirstOrderPicked = User.SpecialOrdersUser.Any(item => item.Order.Id == 2);
            MenuMessageId = await MessageController.SendMessage(User.ChatId, Messages.main_menu,
                Keyboard.Menu(isFirstOrderPicked), ParseMode.Html);
        }

        public async Task SendDailyTaskProgress(string message)
        {
            if (User.IsBlocked) return;
            if (DailyTaskProgressMessageId != -1)
                await DeleteMessage(User.ChatId, DailyTaskProgressMessageId);
            DailyTaskProgressMessageId = await MessageController.SendMessage(User.ChatId, message);
            ChatMessages.Add(DailyTaskProgressMessageId);
        }

        public async Task SendDailyTaskAlert()
        {
            if (User.IsBlocked) return;
            if (DailyTaskAlertMessageId != -1) await DeleteMessage(User.ChatId, DailyTaskAlertMessageId);
            DailyTaskAlertMessageId = await MessageController.SendMessage(User.ChatId, Messages.daily_task_alertation);
            ChatMessages.Add(DailyTaskAlertMessageId);
        }

        public async Task SendDailyTaskComplete()
        {
            if (User.IsBlocked) return;
            await ClearChat();
            if (DailyTaskMessageId != -1) await DeleteMessage(User.ChatId, DailyTaskMessageId);
            DailyTaskMessageId = await MessageController.SendMessage(User.ChatId, Messages.pack_prize, Keyboard.MyPacks);
            ChatMessages.Add(DailyTaskMessageId);
        }

        public async Task SendTopUsers(string message, InlineKeyboardMarkup keyboard)
        {
            if (User.IsBlocked) return;
            if (TopUsersMessageId != -1) await DeleteMessage(User.ChatId, TopUsersMessageId);
            TopUsersMessageId = await MessageController.SendMessage(User.ChatId, message, keyboard, ParseMode.Html);
            ChatMessages.Add(TopUsersMessageId);
        }

        public async Task SendPiggyBankAlert(string message)
        {
            if (User.IsBlocked) return;
            if (CollectIncomeMessageId != -1) await DeleteMessage(User.ChatId, CollectIncomeMessageId);
            CollectIncomeMessageId = await MessageController.SendMessage(User.ChatId, message);
            ChatMessages.Add(CollectIncomeMessageId);
        }

        public async Task SendDocument(InputFileStream file, InlineKeyboardMarkup? keyboard = null)
        {
            if (User.IsBlocked) return;
            ChatMessages.Add(await MessageController.SendDocument(User.ChatId, file, keyboard));
        }
    }
}