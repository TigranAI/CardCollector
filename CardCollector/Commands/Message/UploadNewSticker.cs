using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    public class UploadNewSticker : MessageHandler

    {
        protected override string CommandText => "";

        public override async Task Execute()
        {
            var module = User.Session.GetModule<AdminModule>();
            var newSticker = new StickerEntity(module.SelectedSticker, 
                Update.Message!.Sticker!.FileId, Update.Message!.Sticker!.IsAnimated);
            newSticker = await StickerDao.AddNew(newSticker);
            var stickers = await UserStickerRelationDao.GetListWhere(item => 
                item.StickerId == module.SelectedSticker.Id);
            foreach (var sticker in stickers)
            {
                sticker.StickerId = newSticker.Id;
                sticker.ShortHash = newSticker.Md5Hash;
            }
            await StickerDao.DeleteSticker(module.SelectedSticker);
            await BotDatabase.SaveData();
            await MessageController.EditMessage(User, Messages.update_sticker_success, Keyboard.BackAndMoreKeyboard);
        }

        protected internal override bool Match(UserEntity user, Update update)
        {
            if (update.Message!.Type is not MessageType.Sticker) return false;
            if (user.Session.State is not UserState.EditSticker) return false;
            if (user.Session.GetModule<AdminModule>().SelectedSticker == null) return false;
            return true;
        }

        public UploadNewSticker(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}