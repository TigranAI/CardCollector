using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SelectForSaleSticker : ChosenInlineResultCommand
    {
        protected override string CommandText => Command.select_for_sale_sticker;
        public override async Task Execute()
        {
            var hash = InlineResult.Split('=')[1];
            var sticker = await StickerDao.GetByHash(hash);
            var module = User.Session.GetModule<AdminModule>();
            module.SelectedSticker = sticker;
            await MessageController.SendSticker(User, sticker.Id);
            await MessageController.EditMessage(User, Messages.upload_file_with_watermark, Keyboard.BackKeyboard);
        }

        public SelectForSaleSticker() { }
        public SelectForSaleSticker(UserEntity user, Update update) : base(user, update) { }
    }
}