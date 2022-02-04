using System;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyShopItem : CallbackQueryHandler
    {
        protected override string CommandText => Command.buy_shop_item;

        public override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            var resultPriceCoins = module.SelectedPosition?.ResultPriceCoins 
                                   ?? module.SelectedPack?.PriceCoins * module.Count ?? -1; 
            var resultPriceGems = module.SelectedPosition?.ResultPriceGems 
                                  ?? module.SelectedPack?.PriceGems * module.Count ?? -1;
            var offerExpired = module.SelectedPosition?.Expired ?? false;
            var offerSpecial = module.SelectedPosition?.IsSpecial ?? false;
            var offerInfinite = module.SelectedPosition?.IsInfinite ?? true;
            var offerUsed = module.SelectedPosition != null 
                ? await SpecialOfferUsersDao.NowUsed(User.Id, module.SelectedPosition.Id)
                : false;
            var currency = CallbackData.Split('=')[1];
            if (currency == "coins" && User.Cash.Coins < resultPriceCoins)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_coins, true);
            else if (currency == "gems" && User.Cash.Gems < resultPriceGems)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_gems, true);
            else if (offerExpired)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.offer_expired, true);
            else if (offerSpecial && !offerInfinite && offerUsed)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.you_already_use_this_offer, true);
            else
            {
                await User.Session.ClearStickers();
                if (currency == "coins") User.Cash.Coins -= resultPriceCoins;
                else if (currency == "gems") User.Cash.Gems -= resultPriceGems;
                var canBuy = currency == "coins" && User.Cash.Coins >= resultPriceCoins ||
                             currency == "gems" && User.Cash.Gems >= resultPriceGems;
                if (offerSpecial && !offerInfinite)
                    await SpecialOfferUsersDao.AddNew(User.Id, module.SelectedPosition.Id);
                var packId = module.SelectedPosition?.PackId ?? module.SelectedPack?.Id ?? 1;
                var packInfo = await PacksDao.GetById(packId);
                var userPack = await UserPacksDao.GetOne(User.Id, packId);
                userPack.Count += module.SelectedPosition?.Count ?? module.Count;
                if (module.SelectedPosition?.AdditionalPrize != "") await GivePrize(module.SelectedPosition?.AdditionalPrize);
                await MessageController.EditMessage(User, $"{Messages.thanks_for_buying} {userPack.Count} {Text.packs} {packInfo.Author}", 
                    offerSpecial && !offerInfinite || (module.SelectedPosition?.Expired ?? false) || !canBuy
                    ? Keyboard.BackShopKeyboard
                    : Keyboard.BuyShopItem(CallbackData));
            }
        }

        private async Task GivePrize(string prizeInfo)
        {
            if (prizeInfo == null) return;
            var data = prizeInfo.Split('=');
            StickerEntity sticker = null;
            switch (data[0])
            {
                case "tier":
                    var tier = int.Parse(data[1]);
                    var stickers = await StickerDao.GetListWhere(item => item.Tier == tier);
                    var rnd = new Random();
                    sticker = stickers[rnd.Next(stickers.Count)];
                    break;
                case "sticker":
                    sticker = await StickerDao.GetByHash(data[1]);
                    break;
            }
            if (sticker != null)
            {
                await UserStickerRelationDao.AddSticker(User, sticker);
                await MessageController.SendSticker(User, sticker.Id);
                await MessageController.EditMessage(User, $"{Messages.congratulation}\n{sticker}");
            }
        }

        public BuyShopItem(UserEntity user, Update update) : base(user, update) { }
    }
}