using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database.Entity.NotMapped;
using CardCollector.Resources.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Database.Entity
{
    public class ChatDistribution
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string? ImageFileId { get; set; }
        public string? StickerFileId { get; set; }
        public List<ButtonInfo> Buttons { get; set; } = new();

        public async Task Send(params long[] chatIds)
        {
            foreach (var chatId in chatIds)
            {
                if (StickerFileId != null)
                    await MessageController.SendSticker(chatId, StickerFileId);
                if (ImageFileId != null)
                    await MessageController.SendImage(chatId, ImageFileId, Text, GetKeyboard());
                else
                    await MessageController.SendMessage(chatId, Text, GetKeyboard());
            }
        }

        private InlineKeyboardMarkup? GetKeyboard()
        {
            if (Buttons.Count == 0) return null;
            return new InlineKeyboardMarkup(Buttons.Select(item => item.Type switch {
                ButtonType.Url =>
                    new[] {InlineKeyboardButton.WithUrl(item.Name, item.Value)},
                ButtonType.InlineMenu =>
                    new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(item.Name, item.Value)},
                _ => throw new ArgumentOutOfRangeException()
            }));
        }
    }
}