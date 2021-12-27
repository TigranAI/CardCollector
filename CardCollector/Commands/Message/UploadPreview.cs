using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    public class UploadPreview : MessageCommand
    {
        protected override string CommandText => "";
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            var module = User.Session.GetModule<UploadPreviewModule>();
            module.StickerId = Update.Message.Sticker.FileId;
            module.Animated = Update.Message.Sticker.IsAnimated;
            await MessageController.SendSticker(User, module.StickerId, Keyboard.ConfirmPreview);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return user.PrivilegeLevel >= PrivilegeLevel.Programmer
                   && user.Session.State == UserState.UploadPackPreview
                   && update.Message.Type == MessageType.Sticker;
        }

        public UploadPreview()
        {
        }

        public UploadPreview(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}