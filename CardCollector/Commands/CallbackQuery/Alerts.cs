using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.CallbackQuery
{
    public class Alerts : CallbackQueryCommand
    {
        protected override string CommandText => Command.alerts;
        
        public override async Task Execute()
        {
            var data = CallbackData.Split('=');
            if (data.Length > 1) User.Settings.SwitchProperty((UserSettingsEnum) int.Parse(data[1]));
            await MessageController.EditMessage(User, Messages.alerts, Keyboard.Alerts(User.Settings), ParseMode.Html);
        }

        public Alerts() { }
        public Alerts(UserEntity user, Update update) : base(user, update) { }
    }
}