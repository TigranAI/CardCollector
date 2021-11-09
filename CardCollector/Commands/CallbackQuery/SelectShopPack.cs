using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectShopPack : CallbackQueryCommand
    {
        protected override string CommandText => Command.select_shop_pack;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        private const string PackStickerId = "CAACAgIAAxkBAAIWs2DuY4vB50ARmyRwsgABs_7o5weDaAAC-g4AAmq4cUtH6M1FoN4bxSAE";

        public override async  Task Execute()
        {
            var packId = int.Parse(CallbackData.Split('=')[1]);
            var packInfo = await PacksDao.GetById(packId);
            var module = User.Session.GetModule<ShopModule>();
            module.SelectedPack = packInfo;
            var stickerId = PackStickerId;
            if (packId != 1)
            {
                var stickers = await StickerDao.GetListWhere(item => item.PackId == packId);
                stickerId = stickers[Utilities.rnd.Next(stickers.Count)].Id;
            }
            await MessageController.SendSticker(User, stickerId, Keyboard.OfferKeyboard(module));
        }

        public SelectShopPack() { }
        public SelectShopPack(UserEntity user, Update update) : base(user, update) { }
    }
}