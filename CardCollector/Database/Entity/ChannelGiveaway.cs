using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Resources;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.DataBase.Entity
{
    public class ChannelGiveaway
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public PrizeType Prize { get; set; }
        public int PrizeCount { get; set; }
        public string? Message { get; set; }
        public string? ButtonText { get; set; }
        public string? ImageFileId { get; set; }
        public virtual Sticker? SelectedSticker { get; set; }
        public DateTime? SendAt { get; set; }
        public virtual TelegramChat? Channel { get; set; }
        public virtual ICollection<User> AwardedUsers { get; set; }

        private string PrizeText()
        {
            return Prize switch
            {
                PrizeType.RandomSticker => Text.random_sticker,
                PrizeType.SelectedSticker => SelectedSticker?.Title ?? Text.selected_sticker,
                PrizeType.RandomPack => Text.random_pack,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public string GetFormattedMessage()
        {
            return string.Format(Message, PrizeCount, PrizeText());
        }

        public InlineKeyboardMarkup GetFormattedKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(string.Format(ButtonText, PrizeCount, PrizeText()),
                        CallbackQueryCommands.ignore)
                }
            });
        }

        public enum PrizeType
        {
            RandomSticker,
            SelectedSticker,
            RandomPack
        }
    }
}