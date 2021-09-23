using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyStickerQuery : CallbackQuery
    {
        protected override string CommandText => Command.buy_sticker;
        public override async Task Execute()
        {
            var selectedSticker = User.Session.SelectedSticker;
            var product = await ShopDao.GetSticker(selectedSticker.Id);
            if (selectedSticker.Count > product.Count && !product.IsInfinite)
                await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id, Messages.not_enougth_stickers);
            else if (selectedSticker.Count * selectedSticker.PriceCoins > User.Cash.Coins)
                await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id, Messages.not_enougth_coins);
            else if (selectedSticker.Count * selectedSticker.PriceGems > User.Cash.Gems)
                await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id, Messages.not_enougth_gems);
            else
            {
                if (!product.IsInfinite) product.Count -= selectedSticker.Count;
                if (User.Stickers.ContainsKey(selectedSticker.Md5Hash))
                {
                    await User.Session.PayOutOne(selectedSticker.Md5Hash);
                    await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id, 
                        $"{Messages.you_collected} {User.Session.IncomeCoins}{Text.coin}/{User.Session.IncomeGems}{Text.gem}");
                }
                await UserStickerRelationDao.AddNew(User, selectedSticker, selectedSticker.Count);
                User.Cash.Coins -= selectedSticker.Count * selectedSticker.PriceCoins;
                User.Cash.Gems -= selectedSticker.Count * selectedSticker.PriceGems;
                User.Session.SelectedSticker = null;
                await User.ClearChat();
            }
        }

        public BuyStickerQuery() { }
        public BuyStickerQuery(UserEntity user, Update update) : base(user, update) { }
    }
}