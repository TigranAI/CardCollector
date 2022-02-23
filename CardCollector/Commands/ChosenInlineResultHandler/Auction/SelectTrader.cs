using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler.Auction
{
    public class SelectTrader : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.select_trader;

        protected override async Task Execute()
        {
            var productId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            var product = await Context.Auctions.FindById(productId);
            if (product is null) return;
            var module = User.Session.GetModule<AuctionModule>();
            module.SelectedAuctionId = productId;
            await User.Messages.EditMessage(User, product.Sticker.ToString(product.Count), 
                Keyboard.GetAuctionProductKeyboard(product, User, module.Count));
        }

        public SelectTrader(User user, BotDatabaseContext context, ChosenInlineResult chosenInlineResult) : base(user, context, chosenInlineResult)
        {
        }
    }
}