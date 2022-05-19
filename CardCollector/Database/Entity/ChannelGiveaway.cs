using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Commands.MessageHandler.UrlCommands;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Database.Entity
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
        public int MessageId { get; set; } = -1;
        public DateTime? SendAt { get; set; }
        public virtual Sticker? SelectedSticker { get; set; }
        public int? SelectedStickerTier { get; set; }
        public virtual TelegramChat? Channel { get; set; }
        public virtual ICollection<User> AwardedUsers { get; set; }

        public string PrizeText()
        {
            return Prize switch
            {
                PrizeType.RandomSticker => string.Format(Text.random_sticker_tier, SelectedStickerTier),
                PrizeType.SelectedSticker => SelectedSticker?.Title ?? Text.selected_sticker,
                PrizeType.RandomPack => Text.random_pack,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public string GetFormattedMessage()
        {
            return string.Format(Message, PrizeCount, PrizeText());
        }

        public InlineKeyboardMarkup GetFormattedKeyboard(string? command = null)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(string.Format(ButtonText, PrizeCount, PrizeText()),
                        command ?? $"{CallbackQueryCommands.claim_giveaway}={Id}")
                }
            });
        }

        public bool IsAwarded(long userId)
        {
            return AwardedUsers.Any(item => item.Id == userId);
        }

        public async Task Claim(User user, BotDatabaseContext context)
        {
            PrizeCount--;
            switch (Prize)
            {
                case PrizeType.SelectedSticker:
                    await user.AddSticker(SelectedSticker!, 1, true);
                    break;
                case PrizeType.RandomSticker:
                    var stickers = await context.Stickers.FindAllByTier(SelectedStickerTier!.Value);
                    await user.AddSticker(stickers.Random(), 1, true);
                    break;
                case PrizeType.RandomPack:
                    var pack = await context.Packs.FindById(1);
                    user.AddPack(pack, 1);
                    break;
            }

            AwardedUsers.Add(user);
            await EditMessage();
        }

        public bool IsEnded()
        {
            return PrizeCount <= 0;
        }

        private async Task EditMessage()
        {
            if (PrizeCount <= 0)
            {
                await DeleteMessage(Channel.ChatId, MessageId);
                await SendMessage(Channel.ChatId, Messages.giveaway_now_ended);
            }
            else if (ButtonText == null || ButtonText.Contains("{0}") || ButtonText.Contains("{1}"))
                await Bot.Client.EditMessageReplyMarkupAsync(Channel.ChatId, MessageId, GetFormattedKeyboard());
        }

        public string GetUrl()
        {
            return string.Format(Text.telegram_url_pattern, AppSettings.NAME, MessageUrlCommands.claim_giveaway, Id);
        }

        public async Task<Timer?> Prepare()
        {
            if (SendAt == null) await Send();
            else
            {
                if (PrizeCount <= 0) throw new Exception(Messages.prize_count_must_be_positive);
                var interval = SendAt - DateTime.Now;
                if (interval < TimeSpan.Zero) throw new Exception(Messages.date_is_expired);
                var timer = new Timer()
                {
                    AutoReset = false,
                    Interval = interval.Value.TotalMilliseconds,
                    Enabled = true
                };
                timer.Elapsed += Send;
                return timer;
            }

            return null;
        }

        private async void Send(object? sender, ElapsedEventArgs eventArgs)
        {
            await Send();
            if (sender is Timer timer) timer.Dispose();
        }

        private async Task Send()
        {
            if (Channel.IsBlocked) throw new Exception(Messages.selected_chat_is_blocked);
            if (ImageFileId != null)
            {
                var message = await Bot.Client.SendPhotoAsync(Channel.ChatId,
                    new InputOnlineFile(ImageFileId),
                    GetFormattedMessage(),
                    replyMarkup: GetFormattedKeyboard());
                MessageId = message.MessageId;
            }
            else
            {
                var message = await Bot.Client.SendTextMessageAsync(Channel.ChatId,
                    GetFormattedMessage(),
                    replyMarkup: GetFormattedKeyboard());
                MessageId = message.MessageId;
            }
        }
    }
}