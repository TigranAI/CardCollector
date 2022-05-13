using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    [MenuPoint]
    public class SelectProvider : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.select_provider;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.choose_provider, Keyboard.ProviderKeyboard);
        }
    }
}