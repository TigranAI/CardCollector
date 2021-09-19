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
            User.State = UserState.UploadFile;
            var result = await MessageController.SendMessage(User, Messages.upload_file, Keyboard.CancelKeyboard);
            User.Messages.Add(result.MessageId);
        }

        protected internal override bool IsMatches(string command)
        {
            return User != null 
                ? base.IsMatches(command) && User.PrivilegeLevel >= Constants.ARTIST_PRIVILEGE_LEVEL
                : base.IsMatches(command);
        }

        public DownloadStickerPackMessage() { }
        public DownloadStickerPackMessage(UserEntity user, Update update) : base(user, update) { }
    }
}