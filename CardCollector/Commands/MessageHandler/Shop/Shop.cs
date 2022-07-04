using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.MessageHandler.Shop;

[MenuPoint]
public class Shop : MessageHandler
{
    protected override string CommandText => MessageCommands.shop;
    protected override bool ClearMenu => true;
    protected override bool ClearStickers => true;

    protected override async Task Execute()
    {
        await User.Messages.ClearChat();
        User.Session.ResetModules();

        if (User.OpenStartPack == 0)
        {
            User.Session.State = UserState.ShopMenu;
            var availableSpecialOrders = await Context.SpecialOrders.FindAll();
            var haveSpecialOffers = availableSpecialOrders
                .Any(item => item.IsInfinite || !User.SpecialOrdersUser.Any(usedOrders =>
                                 usedOrders.Order.Id == item.Id));
            await User.Messages.SendMessage(Messages.shop_message,
                Keyboard.ShopKeyboard(haveSpecialOffers, User.PrivilegeLevel));
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

    public override bool Match()
    {
        if (Message.Chat.Type is not (ChatType.Sender or ChatType.Private)) return false;
        if (Message.Type != MessageType.Text) return false;
        return Message.Text == CommandText || Message.Text == $"{CommandText} {Text.gift}";
    }
}