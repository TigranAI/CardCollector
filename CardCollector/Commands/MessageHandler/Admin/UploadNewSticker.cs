using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Admin
{
    public class UploadNewSticker : MessageHandler
    {
        protected override string CommandText => "";

        protected override async Task Execute()
        {
            var stickerId = User.Session.GetModule<AdminModule>().SelectedStickerId;
            var sticker = await Context.Stickers.SingleAsync(item => item.Id == stickerId);
            sticker.FileId = Message.Sticker!.FileId;
            sticker.IsAnimated = Message.Sticker.IsAnimated;
            await User.Messages.EditMessage(User, Messages.update_sticker_success, Keyboard.BackAndMoreKeyboard);
        }

        public override bool Match()
        {
            if (Message.Type is not MessageType.Sticker) return false;
            if (User.Session.State is not UserState.EditSticker) return false;
            if (User.Session.GetModule<AdminModule>().SelectedStickerId == null) return false;
            return true;
        }

        public UploadNewSticker(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}