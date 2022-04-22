using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class UploadPackGifPreview : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.upload_pack_gif_preview;
        protected override async Task Execute()
        {
            User.Session.State = UserState.UploadPackGifPreview;
            var packId = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            User.Session.GetModule<AdminModule>().SelectedPackId = packId;
            await User.Messages.EditMessage(User, Messages.please_upload_preview, Keyboard.BackKeyboard);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }
    }
}