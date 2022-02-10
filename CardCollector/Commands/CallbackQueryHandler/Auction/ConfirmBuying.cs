using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Auction
{
    public class ConfirmBuying : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.confirm_buying;

        protected override async Task Execute()
        {
            var auctionModule = User.Session.GetModule<AuctionModule>();
            var auction = await Context.Auctions.FindById(auctionModule.SelectedAuctionId);
            if (auction == null) return;
            var price = auction.Price * auctionModule.Count ;
            if (price > User.Cash.Gems)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_gems, true);
            else
            {
                var text = $"{Messages.confirm_buying}" +
                           $"\n{auctionModule.Count}{Text.items} {Text.per} {price}{Text.gem}" +
                           $"\n{Messages.are_you_sure}";
                await User.Messages.EditMessage(User, text, Keyboard.GetConfirmationKeyboard(CallbackQueryCommands.buy_sticker));
            }
        }

        public ConfirmBuying(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}