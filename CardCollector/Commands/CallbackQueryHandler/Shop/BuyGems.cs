using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    [MenuPoint]
    public class BuyGems : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.buy_gems;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.buy_gems, Keyboard.BuyGems);
        }
    }
}