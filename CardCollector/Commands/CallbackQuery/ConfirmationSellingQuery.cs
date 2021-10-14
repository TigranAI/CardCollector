using System.Threading.Tasks;
using CardCollector.Commands.Message.TextMessage;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ConfirmationSellingQuery : CallbackQuery
    {
        protected override string CommandText => Command.confirm_selling;
        public override async Task Execute()
        {
            var collectionModule = User.Session.GetModule<CollectionModule>();
            if (collectionModule.SellPrice <= 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.cant_sell_zero, true);
            else
            {
                EnterGemsPriceMessage.RemoveFromQueue(User.Id);
                var income = await User.Cash.Payout(User.Stickers);
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId,
                    $"{Messages.you_collected} {income}{Text.coin}");
                User.Stickers[collectionModule.SelectedSticker.Md5Hash].Count -= collectionModule.Count;
                AuctionController.SellCard(User.Id, collectionModule.SelectedSticker.Id, collectionModule.SellPrice,
                    collectionModule.Count);
                await MessageController.EditMessage(User, CallbackMessageId, Messages.successfully_selling);
            }
        }
        public ConfirmationSellingQuery(){}
        public ConfirmationSellingQuery(UserEntity user, Update update) : base(user, update){}
    }
    
    
}