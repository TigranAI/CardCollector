using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyStickerQuery : CallbackQuery
    {
        protected override string CommandText => Command.buy_sticker;

        public override async Task Execute()
        {
            var auctionModule = User.Session.GetModule<AuctionModule>();
            if (auctionModule.Count > auctionModule.MaxCount)
            {
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_stickers);
                await new BackToFiltersMenu(User, Update).Execute();
            }
            else if (auctionModule.Price * auctionModule.Count > User.Cash.Gems)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_gems);
            else
            {
                await auctionModule.SelectedPosition.BuyCard(auctionModule.Count);
                var discount = 1.0 - await User.AuctionDiscount() / 100.0;
                User.Cash.Gems -= (int)(auctionModule.Price * auctionModule.Count * discount);
                if (!User.Stickers.ContainsKey(auctionModule.SelectedSticker.Md5Hash))
                    await UserStickerRelationDao.AddNew(User, auctionModule.SelectedSticker, auctionModule.Count);
                else
                {
                    await MessageController.AnswerCallbackQuery(User, CallbackQueryId,
                        $"{Messages.you_collected} {await User.Cash.Payout(User.Stickers)}{Text.coin}");
                    User.Stickers[auctionModule.SelectedSticker.Md5Hash].Count += auctionModule.Count;
                }
                User.Session.ResetModule<AuctionModule>();
                await User.ClearChat();
            }
        }

        public BuyStickerQuery() { }
        public BuyStickerQuery(UserEntity user, Update update) : base(user, update) { }
    }
}