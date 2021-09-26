using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class PutForAuctionPart2Query:CallbackQuery
    {
        protected override string CommandText => Command.command_yes_on_auction;
        public override async Task Execute()
        {
            await AuctionController.SellCard(User, User.Session.CoinPrice, User.Session.GemPrice);
            await MessageController.EditMessage(User, CallbackMessageId, "Товар успешно попал на аукцион");

        }
        public PutForAuctionPart2Query(){}
        public PutForAuctionPart2Query(UserEntity user, Update update) : base(user, update){}
    }
    
    
}