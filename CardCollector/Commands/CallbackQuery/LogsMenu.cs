using System;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.CallbackQuery
{
    public class LogsMenu : CallbackQueryCommand
    {
        protected override string CommandText => Command.logs_menu;
        
        public override async Task Execute()
        {
            var date = Convert.ToDateTime(CallbackData.Split('=')[1]);
            var message = string.Format(Messages.logs_on_date, date.ToString("dd.MM.yyyy"));
            //TODO add logs rows
            await MessageController.EditMessage(User, message, Keyboard.LogsMenu(date), ParseMode.Html);
        }

        public LogsMenu() { }
        public LogsMenu(UserEntity user, Update update) : base(user, update) { }
    }
}