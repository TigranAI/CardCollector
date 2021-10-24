using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectOffer : CallbackQueryCommand
    {
        protected override string CommandText => Command.select_offer;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            await User.ClearChat();
            var offerId = int.Parse(CallbackData.Split('=')[1]);
            var offerInfo = await ShopDao.GetById(offerId);
            var module = User.Session.GetModule<ShopModule>();
            module.SelectedPosition = offerInfo;
            var message = await MessageController.SendSticker(User, offerInfo.ImageId, Keyboard.OfferKeyboard(module));
            User.Session.Messages.Add(message.MessageId);
        }

        public SelectOffer() { }
        public SelectOffer(UserEntity user, Update update) : base(user, update) { }
    }
}