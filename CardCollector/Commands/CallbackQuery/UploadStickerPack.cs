using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class UploadStickerPack : CallbackQueryHandler
    {
        protected override string CommandText => Command.upload_stickerpack;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            await MessageController.EditMessage(User, Messages.upload_your_stickers, Keyboard.BackKeyboard);
        }

        protected internal override bool Match(UserEntity user, Update update)
        {
            return base.Match(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Artist;
        }

        public UploadStickerPack(UserEntity user, Update update) : base(user, update) 
        {
            User.Session.State = UserState.UploadSticker;
        }
    }
}