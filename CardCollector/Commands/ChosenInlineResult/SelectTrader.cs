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

        public override async Task Execute()
        {
            var productId = int.Parse(InlineResult.Split('=')[1]);
            var product = await AuctionDao.GetProduct(productId);
            var module = User.Session.GetModule<AuctionModule>();
            if (module.SelectedSticker is not {} sticker) return;
            module.SelectedPosition = product;
            var discount = 1.0 - await User.AuctionDiscount() / 100.0;
            await MessageController.EditMessage(User, sticker.ToString(module.MaxCount), 
                Keyboard.GetStickerKeyboard(User.Session, discount));
        }

        public SelectTrader() { }
        public SelectTrader(UserEntity user, Update update) : base(user, update) { }
    }
}