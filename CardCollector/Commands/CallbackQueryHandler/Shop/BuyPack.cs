using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    [MenuPoint]
    public class BuyPack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.buy_pack;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.choose_option, Keyboard.ShopPacksKeyboard);
        }
    }
}