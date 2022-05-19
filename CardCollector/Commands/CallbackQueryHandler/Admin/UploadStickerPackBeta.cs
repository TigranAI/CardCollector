using System.IO;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.InputFiles;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    [MenuPoint]
    public class UploadStickerPackBeta : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.upload_stickerpack_beta;

        protected override async Task Execute()
        {
            User.Session.State = UserState.UploadStickerPackZip;
            var readme = new FileStream("./stickerpack/readme.txt", FileMode.Open);
            var sample = new FileStream("./stickerpack/sample.zip", FileMode.Open);
            await User.Messages.ClearChat(User);
            await User.Messages.SendDocument(User, new InputFileStream(readme, "readme.txt"));
            await User.Messages.SendDocument(User, new InputFileStream(sample, "sample.zip"));
            await User.Messages.SendMessage(User, Messages.upload_stickerpack_instructions, Keyboard.BackKeyboard);
            readme.Close();
            sample.Close();
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Artist;
        }
    }
}