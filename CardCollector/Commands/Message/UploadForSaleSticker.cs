using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    public class UploadForSaleSticker : MessageCommand
    {
        protected override string CommandText => "";
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            var stickerId = Update.Message!.Sticker!.FileId;
            User.Session.GetModule<AdminModule>().SelectedSticker.ForSaleId = stickerId;
            await BotDatabase.SaveData();
            await MessageController.EditMessage(User, Messages.add_watermark_success, Keyboard.BackAndMoreKeyboard);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            if (update.Message!.Type is not MessageType.Sticker) return false;
            if (user.Session.State is not UserState.LoadForSaleSticker) return false;
            if (user.Session.GetModule<AdminModule>().SelectedSticker == null) return false;
            return true;
        }

        public UploadForSaleSticker() { }
        public UploadForSaleSticker(UserEntity user, Update update) : base(user, update) { }
    }
}