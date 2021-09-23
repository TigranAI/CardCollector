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
            User.Session.SelectedSticker.TraderInfo = trader;
            var sticker = User.Session.SelectedSticker;
            var text = $"\n<<{sticker.Title}>>" +
                       $"\n{Text.emoji}: {sticker.Emoji}" +
                       $"\n{Text.author}: {sticker.Author}" +
                       $"\n{Text.count}: {sticker.TraderInfo.Quantity}" +
                       $"\n{sticker.IncomeCoins}{Text.coin} / {sticker.IncomeGems}{Text.gem} {sticker.IncomeTime}{Text.time}{Text.minutes}";
            var messageSticker = await MessageController.SendSticker(User, User.Session.SelectedSticker.Id);
            var message = await MessageController.SendMessage(User, text, Keyboard.GetStickerKeyboard(User.Session));
            User.Session.Messages.Add(messageSticker.MessageId);
            User.Session.Messages.Add(message.MessageId);
        }

        public SelectTraderResult() { }
        public SelectTraderResult(UserEntity user, Update update, string inlineResult) : base(user, update, inlineResult) { }
    }
}