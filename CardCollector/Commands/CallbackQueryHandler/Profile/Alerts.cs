using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    public class Alerts : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.alerts;

        protected override async Task Execute()
        {
            var data = CallbackQuery.Data!.Split('=');
            if (data.Length > 1) User.Settings.SwitchProperty((UserSettings) int.Parse(data[1]));
            await User.Messages.EditMessage(User, Messages.alerts, Keyboard.Alerts(User.Settings), ParseMode.Html);
        }
    }
}