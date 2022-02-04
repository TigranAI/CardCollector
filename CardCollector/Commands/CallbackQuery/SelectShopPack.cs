using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectShopPack : CallbackQueryHandler
    {
        protected override string CommandText => Command.select_shop_pack;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            var packId = int.Parse(CallbackData.Split('=')[1]);
            var packInfo = await PacksDao.GetById(packId);
            var module = User.Session.GetModule<ShopModule>();
            module.SelectedPack = packInfo;
            await MessageController.SendSticker(User, packInfo.StickerPreview, Keyboard.OfferKeyboard(module));
        }

        public SelectShopPack(UserEntity user, Update update) : base(user, update) { }
    }
}