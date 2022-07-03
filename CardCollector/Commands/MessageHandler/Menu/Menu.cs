using System.Threading.Tasks;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.MessageHandler.Menu;

public class Menu : MessageHandler
{
    protected override string CommandText => MessageCommands.menu;

    protected override async Task Execute()
    {
        await User.Messages.ClearChat();
        if (User.OpenStartPack == 0)
        {
            await User.Messages.SendMenu();
        }
        else
        {
            var packInfo = await Context.Packs.FindById(1);
            await User.Messages.SendSticker(packInfo.PreviewFileId, OpenStartPacks());
            if (!User.FirstReward)
            {
                User.FirstReward = true;
                User.AddPack(packInfo, 7);
                await User.Messages.SendSticker(packInfo.PreviewFileId!, OpenStartPacks());
            }
        }
    }

    private InlineKeyboardMarkup OpenStartPacks()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_start_packs, CallbackQueryCommands.open_pack)}
        });
    }
}