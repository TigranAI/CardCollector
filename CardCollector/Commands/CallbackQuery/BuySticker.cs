using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuySticker : CallbackQueryCommand
    {
        protected override string CommandText => Command.buy_sticker;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            var auctionModule = User.Session.GetModule<AuctionModule>();
            if (auctionModule.Count > auctionModule.MaxCount)
            {
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_stickers);
                await new Back(User, Update).PrepareAndExecute();
            }
            else if (auctionModule.Price * auctionModule.Count > User.Cash.Gems)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_gems);
            else
            {
                await auctionModule.SelectedPosition.BuyCard(auctionModule.Count);
                var discount = 1.0 - await User.AuctionDiscount() / 100.0;
                User.Cash.Gems -= (int)(auctionModule.Price * auctionModule.Count * discount);
                await UserStickerRelationDao.AddSticker(User, auctionModule.SelectedSticker, auctionModule.Count);
                User.Session.ResetModule<AuctionModule>();
            }
        }

        public BuySticker() { }
        public BuySticker(UserEntity user, Update update) : base(user, update) { }
    }
}