using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    public class DownloadStickerPack : MessageCommand
    {
        protected override string CommandText => Text.download_stickerpack;
        protected override bool ClearMenu => true;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            await MessageController.EditMessage(User, Messages.upload_your_stickers, Keyboard.BackKeyboard);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Artist;
        }

        public DownloadStickerPack() { }
        public DownloadStickerPack(UserEntity user, Update update) : base(user, update) 
        {
            User.Session.State = UserState.UploadSticker;
        }
    }
}