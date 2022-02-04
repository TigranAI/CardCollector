using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class EndUploadStickers : CallbackQueryHandler
    {
        protected override string CommandText => Command.end_sticker_upload;

        public override async Task Execute()
        {
            await MessageController.EditMessage(User, Messages.upload_your_file, Keyboard.BackKeyboard);
        }

        public EndUploadStickers(UserEntity user, Update update) : base(user, update)
        {
            User.Session.State = UserState.UploadFile;
        }
    }
}