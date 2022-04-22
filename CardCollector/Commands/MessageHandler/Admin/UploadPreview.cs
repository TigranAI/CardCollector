using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Admin
{
    public class UploadPreview : MessageHandler
    {
        protected override string CommandText => "";
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var packId = User.Session.GetModule<AdminModule>().SelectedPackId;
            var pack = await Context.Packs.FindById(packId);
            pack.PreviewFileId = Message.Sticker!.FileId;
            pack.IsPreviewAnimated = Message.Sticker.IsAnimated;
            await User.Messages.EditMessage(User, Messages.update_preview_success, Keyboard.BackKeyboard);
            
            await Context.SaveChangesAsync();
            await new RequestBuilder()
                .SetUrl("recache")
                .AddParam("packId", pack.Id)
                .AddParam("type", (int) RecacheType.UploadPackPreview)
                .Send();
        }

        public override bool Match()
        {
            if (User.PrivilegeLevel < PrivilegeLevel.Programmer) return false;
            if (User.Session.State != UserState.UploadPackPreview) return false;
            if (Message.Type is not MessageType.Sticker) return false;
            if (User.Session.GetModule<AdminModule>().SelectedPackId == null) return false;
            return true;
        }
    }
}