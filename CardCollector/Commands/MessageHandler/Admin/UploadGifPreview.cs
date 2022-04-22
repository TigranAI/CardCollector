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
    public class UploadGifPreview : MessageHandler
    {
        protected override string CommandText => "";
        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AdminModule>();
            var pack = await Context.Packs.FindById(module.SelectedPackId);
            pack.GifPreviewFileId = Message.Document!.FileId;
            await User.Messages.ClearChat(User);
            await User.Messages.SendMessage(User, Messages.update_preview_success, Keyboard.BackKeyboard);
            
            await Context.SaveChangesAsync();
            await new RequestBuilder()
                .SetUrl("recache")
                .AddParam("packId", pack.Id)
                .AddParam("type", (int) RecacheType.UploadPackGifPreview)
                .Send();
        }

        public override bool Match()
        {
            Logs.LogOut(Message.Type);
            Logs.LogOut(Message.Document?.MimeType);
            return User.PrivilegeLevel >= PrivilegeLevel.Programmer
                   && User.Session.State is UserState.UploadPackGifPreview
                   && Message.Type is MessageType.Document
                   && Message.Document!.MimeType is "image/gif";
        }
    }
}