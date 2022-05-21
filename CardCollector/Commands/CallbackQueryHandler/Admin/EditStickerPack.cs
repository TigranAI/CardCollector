using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.CallbackQueryHandler.Admin;

[MenuPoint]
public class EditStickerPack : CallbackQueryHandler
{
    protected override string CommandText => CallbackQueryCommands.edit_stickerpack;

    protected override async Task Execute()
    {
        var data = CallbackQuery.Data!.Split("=");
        await User.Messages.EditMessage(Messages.choose_action, Keyboard(data[1]));
    }

    private InlineKeyboardMarkup Keyboard(string packId)
    {
        var keyboard = new List<InlineKeyboardButton[]>();
        if (User.PrivilegeLevel == PrivilegeLevel.Programmer)
            keyboard.AddRange(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.add_for_sale_sticker,
                        $"{CallbackQueryCommands.select_for_sale_pack}={packId}")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.add_sticker_preview,
                        $"{CallbackQueryCommands.add_sticker_preview}={packId}")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.edit_sticker,
                        $"{CallbackQueryCommands.edit_sticker}={packId}")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.add_puzzle,
                        $"{CallbackQueryCommands.add_puzzle}={packId}")
                }
            });
        if (User.PrivilegeLevel >= PrivilegeLevel.Programmer)
            keyboard.AddRange(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.upload_pack_gif_preview,
                        $"{CallbackQueryCommands.upload_pack_gif_preview}={packId}")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)
                }
            });
        return new InlineKeyboardMarkup(keyboard);
    }
}