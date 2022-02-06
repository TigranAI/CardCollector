using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Admin
{
    public class UploadForSaleSticker : MessageHandler
    {
        protected override string CommandText => "";
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var stickerId = User.Session.GetModule<AdminModule>().SelectedStickerId;
            var sticker = await Context.Stickers.SingleAsync(item => item.Id == stickerId);
            sticker.ForSaleFileId = Message.Sticker!.FileId;
            sticker.IsForSaleAnimated = Message.Sticker.IsAnimated;
            await User.Messages.EditMessage(User, Messages.add_watermark_success, Keyboard.BackAndMoreKeyboard);
        }

        public override bool Match()
        {
            if (Message.Type is not MessageType.Sticker) return false;
            if (User.Session.State is not UserState.LoadForSaleSticker) return false;
            if (User.Session.GetModule<AdminModule>().SelectedStickerId == null) return false;
            return true;
        }

        public UploadForSaleSticker(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}