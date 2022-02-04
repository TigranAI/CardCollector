using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectOffer : CallbackQueryHandler
    {
        protected override string CommandText => Command.select_offer;

        public override async Task Execute()
        {
            var offerId = int.Parse(CallbackData.Split('=')[1]);
            var offerInfo = await ShopDao.GetById(offerId);
            var module = User.Session.GetModule<ShopModule>();
            module.SelectedPosition = offerInfo;
            await MessageController.SendSticker(User, offerInfo.ImageId, Keyboard.OfferKeyboard(module));
        }

        public SelectOffer(UserEntity user, Update update) : base(user, update) { }
    }
}