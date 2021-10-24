using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    public class UploadSticker : MessageCommand
    {
        protected override string CommandText => "";
        public override async Task Execute()
        {
            var stickerId = Update.Message?.Sticker?.FileId;
            var module = User.Session.GetModule<UploadedStickersModule>();
            module.StickersList.Add(new StickerEntity {Id = stickerId});
            var message = $"{Messages.upload_your_stickers}" +
                          $"\n{Messages.uploaded_count} {module.Count}";
            foreach (var (stickerEntity, i) in module.StickersList.WithIndex())
                message += $"\n{Text.sticker} {i + 1}: {stickerEntity.Id}";
            await MessageController.EditMessage(User, module.MessageId, message, Keyboard.EndStickerUpload);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return user.Session.State == UserState.UploadSticker && update.Message?.Type == MessageType.Sticker;
        }

        public UploadSticker() { }
        public UploadSticker(UserEntity user, Update update) : base(user, update) { }
    }
}