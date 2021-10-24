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
        public override async Task Execute()
        {
            User.Session.State = UserState.UploadFile;
            var module = User.Session.GetModule<UploadedStickersModule>();
            await MessageController.EditMessage(User, module.MessageId, Messages.upload_your_file, 
                Keyboard.CancelKeyboard);
        }

        public EndUploadStickers() { }
        public EndUploadStickers(UserEntity user, Update update) : base(user, update) { }
    }
}