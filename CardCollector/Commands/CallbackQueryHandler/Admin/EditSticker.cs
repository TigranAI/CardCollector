using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    [MenuPoint]
    public class EditSticker : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.edit_sticker;
        protected override bool ClearStickers => true;
        protected override async Task Execute()
        {
            User.Session.State = UserState.EditSticker;
            User.Session.GetModule<AdminModule>().SelectedPackId = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            await User.Messages.EditMessage(User, Messages.select_sticker, Keyboard.ShowStickers);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }
    }
}