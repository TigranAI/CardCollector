using System.Threading.Tasks;
using CardCollector.Commands.CallbackQueryHandler.Others;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQueryHandler.Auction
{
    public class BuySticker : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.buy_sticker;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AuctionModule>();
            var productInfo = await Context.Auctions.FindById(module.SelectedAuctionId);
            if (productInfo == null || module.Count > productInfo.Count)
            {
                await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_stickers,
                    true);
                await new Back().Init(User, Context, new Update() {CallbackQuery = CallbackQuery}).PrepareAndExecute();
            }
            else if (productInfo.Price * module.Count > User.Cash.Gems)
                await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_gems, true);
            else
            {
                productInfo.Count -= module.Count;
                if (productInfo.Count == 0)
                {
                    Context.Auctions.Attach(productInfo);
                    Context.Auctions.Remove(productInfo);
                }

                var sum = productInfo.Price * module.Count;
                
                await productInfo.Trader.Messages.SendMessage(productInfo.Trader,
                    string.Format(Messages.thanks_for_selling_sticker, User.Username, productInfo.Sticker.Title,
                        module.Count, (int) (sum * 0.70)));
                await productInfo.Trader.AddGems((int) (sum * 0.7));
                
                if (User.HasAuctionDiscount()) sum = (int) (sum * 0.95);
                User.Cash.Gems -= sum;
                await User.Messages.EditMessage(User, string.Format(Messages.thanks_for_buying_sticker,
                    productInfo.Trader.Username, productInfo.Sticker.Title), Keyboard.BackKeyboard);

                await User.AddSticker(productInfo.Sticker, module.Count);
                User.Session.ResetModule<AuctionModule>();
                
                if (User.InviteInfo?.TasksProgress is { } tp && !tp.BuyStickerOnAuction)
                {
                    tp.BuyStickerOnAuction = true;
                    await User.InviteInfo.CheckRewards(Context);
                }
            }
        }
    }
}