using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class PackInfo : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.pack_info;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(Messages.pack_info, Keyboard.BackKeyboard);
        }
    }
}