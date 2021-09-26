using System.Threading.Tasks;
using CardCollector.Commands.Message.TextMessage;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ConfirmationSellingQuery : CallbackQuery
    {
        protected override string CommandText => Command.confirm_selling;
        public override async Task Execute()
        {
            EnterCoinsPriceMessage.RemoveFromQueue(User.Id);
            EnterGemsPriceMessage.RemoveFromQueue(User.Id);
            await User.Session.PayOutOne(User.Session.SelectedSticker.Md5Hash);
            await MessageController.AnswerCallbackQuery(User, CallbackQueryId, 
                $"{Messages.you_collected} {User.Session.IncomeCoins}{Text.coin} / {User.Session.IncomeGems}{Text.gem}");
            AuctionController.SellCard(User, User.Session.CoinPrice, User.Session.GemPrice);
            await MessageController.EditMessage(User, CallbackMessageId, Messages.successfully_selling);
        }
        public ConfirmationSellingQuery(){}
        public ConfirmationSellingQuery(UserEntity user, Update update) : base(user, update){}
    }
    
    
}