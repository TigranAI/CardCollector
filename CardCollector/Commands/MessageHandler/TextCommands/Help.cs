using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.MessageHandler.TextCommands;

[MenuPoint]
public class Help : MessageHandler
{
    protected override string CommandText => MessageCommands.help;
    protected override async Task Execute()
    {
        await User.Messages.ClearChat();
        await User.Messages.EditMessage(Messages.help);
        var packInfo = await Context.Packs.FindById(1);
        if (User.OpenStartPack > 0)
            await User.Messages.SendSticker(packInfo.PreviewFileId!, OpenStartPacks());
    }

    private InlineKeyboardMarkup OpenStartPacks()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_start_packs, CallbackQueryCommands.open_pack)}
        });
    }
}