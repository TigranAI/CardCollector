using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SelectTrader : ChosenInlineResultCommand
    {
        protected override string CommandText => Command.buy_sticker;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            await User.ClearChat();
            var productId = int.Parse(InlineResult.Split('=')[1]);
            var product = await AuctionDao.GetProduct(productId);
            var module = User.Session.GetModule<AuctionModule>();
            if (module.SelectedSticker is not {} sticker) return;
            module.SelectedPosition = product;
            var discount = 1.0 - await User.AuctionDiscount() / 100.0;
            var messageSticker = await MessageController.SendSticker(User, sticker.Id);
            var message = await MessageController.SendMessage(User, sticker.ToString(module.MaxCount), 
                Keyboard.GetStickerKeyboard(User.Session, discount));
            User.Session.Messages.Add(messageSticker.MessageId);
            User.Session.Messages.Add(message.MessageId);
        }

        public SelectTrader() { }
        public SelectTrader(UserEntity user, Update update) : base(user, update) { }
    }
}