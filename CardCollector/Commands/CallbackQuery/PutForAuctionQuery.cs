using System.Threading.Tasks;
using CardCollector.Commands.Message.TextMessage;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class PutForAuctionQuery : CallbackQuery
    {
        protected override string CommandText => Command.sell_on_auction;

        public override async Task Execute()
        {
            await User.ClearChat();
            User.Session.CoinPrice = User.Session.SelectedSticker.PriceCoins;
            User.Session.GemPrice = User.Session.SelectedSticker.PriceGems;
            var message = await MessageController.SendMessage(User,
                $"{Messages.current_price} {User.Session.CoinPrice}{Text.coin} / {User.Session.GemPrice}{Text.gem}" +
                $"\n{Messages.enter_your_coins_price} {Text.coin}:", Keyboard.AuctionPutCancelKeyboard);
            EnterCoinsPriceMessage.AddToQueue(User.Id, message.MessageId);
            User.Session.Messages.Add(message.MessageId);
        }

        public PutForAuctionQuery() { }
        public PutForAuctionQuery(UserEntity user, Update update) : base(user, update) { }
    }
}