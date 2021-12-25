using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class UploadStickerPack : CallbackQueryCommand
    {
        protected override string CommandText => Command.upload_stickerpack;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            await MessageController.EditMessage(User, Messages.upload_your_stickers, Keyboard.BackKeyboard);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Artist;
        }

        public UploadStickerPack() { }
        public UploadStickerPack(UserEntity user, Update update) : base(user, update) 
        {
            User.Session.State = UserState.UploadSticker;
        }
    }
}