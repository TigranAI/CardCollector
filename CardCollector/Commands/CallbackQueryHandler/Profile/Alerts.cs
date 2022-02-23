using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    public class Alerts : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.alerts;

        protected override async Task Execute()
        {
            var data = CallbackQuery.Data!.Split('=');
            if (data.Length > 1) User.Settings.SwitchProperty((Resources.Enums.UserSettings) int.Parse(data[1]));
            await User.Messages.EditMessage(User, Messages.alerts, Keyboard.Alerts(User.Settings), ParseMode.Html);
        }

        public Alerts(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}