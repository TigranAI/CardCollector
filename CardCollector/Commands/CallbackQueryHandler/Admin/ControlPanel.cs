using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Attributes.Menu;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

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

        public ControlPanel(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}