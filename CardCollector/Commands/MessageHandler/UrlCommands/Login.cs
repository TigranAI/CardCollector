using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.MessageHandler.UrlCommands;

[MenuPoint]
public class Login : MessageUrlHandler
{
    protected override string CommandText => MessageUrlCommands.confirm_login;

    protected override async Task Execute()
    {
        await User.Messages.ClearChat();
        if (User.OpenStartPack == 0)
        {
            await User.Messages.EditMessage($"{Messages.confirm_login} {AppSettings.SITE_URL}",
                Keyboard.ConfirmLogin(StartData[1]));
        }
        else
        {
            var packInfo = await Context.Packs.FindById(1);
            await User.Messages.SendSticker(packInfo.PreviewFileId, OpenStartPacks());
            if (!User.FirstReward)
            {
                User.FirstReward = true;
                User.AddPack(packInfo, 7);
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