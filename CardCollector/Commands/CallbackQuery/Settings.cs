using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.CallbackQuery
{
    public class Settings : CallbackQueryCommand
    {
        protected override string CommandText => Command.settings;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            await MessageController.EditMessage(User, Messages.settings,
                Keyboard.Settings(User.PrivilegeLevel), ParseMode.Html);
        }

        public Settings() { }
        public Settings(UserEntity user, Update update) : base(user, update) { }
    }
}