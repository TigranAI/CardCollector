using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.CallbackQuery
{
    public class ControlPanel : CallbackQueryHandler
    {
        protected override string CommandText => Command.control_panel;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            await MessageController.EditMessage(User, Messages.control_panel, 
                Keyboard.ControlPanel(User.PrivilegeLevel), ParseMode.Html);
        }

        protected internal override bool Match(UserEntity user, Update update)
        {
            return base.Match(user, update) && user.PrivilegeLevel > PrivilegeLevel.Vip;
        }

        public ControlPanel(UserEntity user, Update update) : base(user, update) { }
    }
}