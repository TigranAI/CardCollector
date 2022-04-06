using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class EndUploadStickers : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.end_sticker_upload;

        protected override async Task Execute()
        {
            User.Session.State = UserState.UploadFile;
            await User.Messages.EditMessage(User, Messages.upload_your_file, Keyboard.BackKeyboard);
        }
    }
}