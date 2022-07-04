using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.MessageHandler.Auction;

[MenuPoint]
public class Auction : MessageHandler
{
    protected override string CommandText => MessageCommands.auction;
    protected override bool ClearMenu => true;
    protected override bool ClearStickers => true;

    protected override async Task Execute()
    {
        await User.Messages.ClearChat();
        User.Session.ResetModules();
        if (User.OpenStartPack == 0)
        {
            User.Session.State = UserState.AuctionMenu;
            var text = User.Session.GetModule<FiltersModule>().ToString(User.Session.State);
            await User.Messages.SendMessage(text, Keyboard.GetSortingMenu(User.Session.State));
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