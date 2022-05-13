using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    [MenuPoint]
    public class Settings : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.settings;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.settings,
                Keyboard.Settings(User.PrivilegeLevel), ParseMode.Html);
        }
    }
}