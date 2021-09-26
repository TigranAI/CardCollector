using System.Threading.Tasks;
using CardCollector.Commands.Message.TextMessage;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.CallbackQuery
{
    public class PutForAuctionQuery:CallbackQuery
    {
        protected override string CommandText => Command.sell_on_auction;
        public override async Task Execute()
        {
            await User.ClearChat();
            var message = await MessageController.SendMessage(User, "Введите монеты", Keyboard.CancelKeyboard);
            EnterPriceMessage.AddToQueue(User.Id, message.MessageId);
            
            

        }
        
        
        public PutForAuctionQuery() {}
        public PutForAuctionQuery(UserEntity user, Update update) : base(user, update) { }
    }
}