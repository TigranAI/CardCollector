using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class EndUploadStickers : CallbackQueryCommand
    {
        protected override string CommandText => Command.end_sticker_upload;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            User.Session.State = UserState.UploadFile;
            await MessageController.EditMessage(User, Messages.upload_your_file, Keyboard.BackKeyboard);
        }

        public EndUploadStickers() { }
        public EndUploadStickers(UserEntity user, Update update) : base(user, update) { }
    }
}