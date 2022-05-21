using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class AddStickerPreview : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.add_sticker_preview;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            User.Session.State = UserState.UploadPackPreview;
            var packId = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            User.Session.GetModule<AdminModule>().SelectedPackId = packId;
            await User.Messages.EditMessage(Messages.please_upload_preview, Keyboard.BackKeyboard);
        }

        public override bool Match()
        {
            Logs.LogOut(base.Match());
            Logs.LogOut(User.PrivilegeLevel >= PrivilegeLevel.Programmer);
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }
    }
}