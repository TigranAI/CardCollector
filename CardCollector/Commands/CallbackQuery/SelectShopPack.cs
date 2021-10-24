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
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async  Task Execute()
        {
            await User.ClearChat();
            var packId = int.Parse(CallbackData.Split('=')[1]);
            var packInfo = await PacksDao.GetById(packId);
            var module = User.Session.GetModule<ShopModule>();
            module.SelectedPack = packInfo;
            var stickers = await StickerDao.GetListWhere(item => packId == 1 || item.PackId == packId);
            var sticker = stickers[Utilities.rnd.Next(stickers.Count)];
            var message = await MessageController.SendSticker(User, sticker.Id, Keyboard.OfferKeyboard(module));
            User.Session.Messages.Add(message.MessageId);
        }

        public SelectShopPack() { }
        public SelectShopPack(UserEntity user, Update update) : base(user, update) { }
    }
}