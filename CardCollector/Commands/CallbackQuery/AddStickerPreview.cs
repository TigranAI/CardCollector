using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class AddStickerPreview : CallbackQueryCommand
    {
        protected override string CommandText => Command.add_sticker_preview;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            User.Session.State = UserState.UploadPackPreview;
            var packId = int.Parse(CallbackData.Split('=')[1]);
            User.Session.GetModule<UploadPreviewModule>().PackId = packId;
            await MessageController.EditMessage(User, Messages.please_upload_preview, Keyboard.BackKeyboard);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public AddStickerPreview()
        {
        }

        public AddStickerPreview(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}