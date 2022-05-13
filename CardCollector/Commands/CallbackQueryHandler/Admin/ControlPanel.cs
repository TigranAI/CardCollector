using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    [MenuPoint]
    public class ControlPanel : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.control_panel;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.control_panel, 
                Keyboard.ControlPanel(User.PrivilegeLevel), ParseMode.Html);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }
    }
}