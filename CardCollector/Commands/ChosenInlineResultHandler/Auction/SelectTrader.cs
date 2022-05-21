using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;

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
            module.Count = 1;
            await User.Messages.EditMessage(product.Sticker.ToString(product.Count), 
                Keyboard.GetAuctionProductKeyboard(product, User, module.Count));
        }
    }
}