using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SelectTraderResult : ChosenInlineResult
    {
        protected override string CommandText => Command.buy_sticker;
        public override async Task Execute()
        {
            await User.ClearChat();
            var productId = int.Parse(InlineResult.Split('=')[1]);
            var trader = await AuctionDao.GetTraderInfo(productId);
            if (User.Session.SelectedSticker == null) return;
            var sticker = User.Session.SelectedSticker;
            sticker.TraderInfo = trader;
            sticker.MaxCount = trader.Quantity;
            var messageSticker = await MessageController.SendSticker(User, User.Session.SelectedSticker.Id);
            var message = await MessageController.SendMessage(User, sticker.ToString(), Keyboard.GetStickerKeyboard(User.Session));
            User.Session.Messages.Add(messageSticker.MessageId);
            User.Session.Messages.Add(message.MessageId);
        }

        public SelectTraderResult() { }
        public SelectTraderResult(UserEntity user, Update update) : base(user, update) { }
    }
}