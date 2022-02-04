using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ReturnFromAuction : CallbackQueryHandler
    {
        protected override string CommandText => Command.return_from_auction;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            var module = User.Session.GetModule<AuctionModule>();
            var productInfo = await AuctionDao.GetProduct(module.SelectedPosition.Id);
            await AuctionDao.DeleteRow(productInfo.Id);
            await UserStickerRelationDao.AddSticker(User, module.SelectedSticker, productInfo.Count);
            await MessageController.EditMessage(User,
                string.Format(Messages.successfully_returned, productInfo.Count, module.SelectedSticker.Title),
                Keyboard.BackKeyboard);
        }

        public ReturnFromAuction(UserEntity user, Update update) : base(user, update) { }
    }
}