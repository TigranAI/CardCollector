using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    public class DownloadStickerPackMessage : Message
    {
        protected override string CommandText => Text.download_stickerpack;
        public override async Task Execute()
        {
            await User.ClearChat();
            User.Session.State = UserState.UploadFile;
            var result = await MessageController.SendMessage(User, Messages.upload_file, Keyboard.CancelKeyboard);
            User.Session.Messages.Add(result.MessageId);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && user.PrivilegeLevel >= Constants.ARTIST_PRIVILEGE_LEVEL;
        }

        public DownloadStickerPackMessage() { }
        public DownloadStickerPackMessage(UserEntity user, Update update) : base(user, update) { }
    }
}