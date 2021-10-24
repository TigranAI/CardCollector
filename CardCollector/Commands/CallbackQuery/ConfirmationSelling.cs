using System.Threading.Tasks;
using CardCollector.Commands.Message;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ConfirmationSelling : CallbackQueryCommand
    {
        protected override string CommandText => Command.confirm_selling;
        public override async Task Execute()
        {
            var collectionModule = User.Session.GetModule<CollectionModule>();
            if (collectionModule.SellPrice <= 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.cant_sell_zero, true);
            else
            {
                EnterGemsPrice.RemoveFromQueue(User.Id);
                User.Stickers[collectionModule.SelectedSticker.Md5Hash].Count -= collectionModule.Count;
                AuctionController.SellCard(User.Id, collectionModule.SelectedSticker.Id, collectionModule.SellPrice,
                    collectionModule.Count);
                await MessageController.EditMessage(User, CallbackMessageId, Messages.successfully_selling);
            }
        }
        
        public ConfirmationSelling(){}
        public ConfirmationSelling(UserEntity user, Update update) : base(user, update){}
    }
    
    
}