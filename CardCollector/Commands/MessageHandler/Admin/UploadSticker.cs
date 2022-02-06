using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Sticker = CardCollector.DataBase.Entity.Sticker;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Admin
{
    public class UploadSticker : MessageHandler
    {
        protected override string CommandText => "";

        protected override async Task Execute()
        {
            var sticker = new Sticker() {FileId = Message.Sticker!.FileId, IsAnimated = Message.Sticker!.IsAnimated};
            var module = User.Session.GetModule<AdminModule>();
            module.StickersList.Add(sticker);
            var message = $"{Messages.upload_your_stickers}" +
                          $"\n{Messages.uploaded_count} {module.StickersList.Count}";
            foreach (var (stickerEntity, i) in module.StickersList.WithIndex())
                message += $"\n{Text.sticker} {i + 1}: {stickerEntity.Id}";
            await User.Messages.EditMessage(User, message, Keyboard.EndStickerUpload);
        }

        public override bool Match()
        {
            if (User.Session.State != UserState.UploadSticker) return false;
            if (Message.Type is not MessageType.Sticker) return false;
            return true;
        }

        public UploadSticker(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}