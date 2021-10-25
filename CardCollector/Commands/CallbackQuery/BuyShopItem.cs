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
    public class BuyShopItem : CallbackQueryCommand
    {
        protected override string CommandText => Command.buy_shop_item;
        protected override bool ClearMenu => true;
        protected override bool AddToStack => false;

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
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_coins);
            else if (currency == "gems" && User.Cash.Gems < resultPriceGems)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_gems);
            else if (offerExpired)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.offer_expired);
            else if (offerSpecial && !offerInfinite && offerUsed)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.you_already_use_this_offer);
            else
            {
                await User.ClearChat();
                if (currency == "coins") User.Cash.Coins -= resultPriceCoins;
                else if (currency == "gems") User.Cash.Gems -= resultPriceGems;
                
                if (offerSpecial && !offerInfinite)
                    await SpecialOfferUsersDao.AddNew(User.Id, module.SelectedPosition.Id);
                var packId = module.SelectedPosition?.PackId ?? module.SelectedPack?.Id ?? 1;
                var userPack = await UserPacksDao.GetOne(User.Id, packId);
                userPack.Count += module.SelectedPosition?.Count ?? module.Count;
                if (module.SelectedPosition?.AdditionalPrize != "") await GivePrize(module.SelectedPosition?.AdditionalPrize);
                var message = await MessageController.SendMessage(User, Messages.thanks_for_buying);
                User.Session.ResetModule<ShopModule>();
                User.Session.Messages.Add(message.MessageId);
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
                if (!User.Stickers.ContainsKey(sticker.Md5Hash))
                    await UserStickerRelationDao.AddNew(User, sticker, 1);
                else
                    User.Stickers[sticker.Md5Hash].Count ++;
                var stickerMessage = await MessageController.SendSticker(User, sticker.Id);
                var message = await MessageController.SendMessage(User, $"{Messages.congratulation}\n{sticker}");
                User.Session.Messages.Add(stickerMessage.MessageId);
                User.Session.Messages.Add(message.MessageId);
            }
        }

        public BuyShopItem() { }
        public BuyShopItem(UserEntity user, Update update) : base(user, update) { }
    }
}