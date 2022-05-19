using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    [MenuPoint]
    public class ControlPanel : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.control_panel;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.control_panel, Keyboard(User.PrivilegeLevel), ParseMode.Html);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }
        
        private InlineKeyboardMarkup Keyboard(PrivilegeLevel level)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            if (level >= PrivilegeLevel.Programmer)
                keyboard.AddRange(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.logs_menu,
                            $"{CallbackQueryCommands.logs_menu}={DateTime.Today}")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.create_giveaway,
                            $"{CallbackQueryCommands.create_giveaway}")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.create_distribution,
                            $"{CallbackQueryCommands.create_distribution}")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.upload_pack_gif_preview,
                            $"{CallbackQueryCommands.choose_pack}={CallbackQueryCommands.upload_pack_gif_preview}=0")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.show_sample, CallbackQueryCommands.show_sample)
                    },
                });
            if (level == PrivilegeLevel.Programmer)
                keyboard.AddRange(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.stop_bot, CallbackQueryCommands.stop_bot)
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Beta " + Text.upload_stickerpack, 
                            CallbackQueryCommands.upload_stickerpack_beta)
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.upload_stickerpack,
                            CallbackQueryCommands.upload_stickerpack)
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.add_for_sale_sticker,
                            $"{CallbackQueryCommands.choose_pack}={CallbackQueryCommands.select_for_sale_pack}=0")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.add_sticker_preview,
                            $"{CallbackQueryCommands.choose_pack}={CallbackQueryCommands.add_sticker_preview}=0")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.edit_sticker,
                            $"{CallbackQueryCommands.choose_pack}={CallbackQueryCommands.edit_sticker}=0")
                    },
                });
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }
    }
}