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
    public class BuyByCoins : CallbackQuery
    {
        protected override string CommandText => Command.buy_by_coins;
        public override async Task Execute()
        {
            var offerInfo = User.Session.GetModule<ShopModule>().SelectedPosition;
            if (User.Cash.Coins < offerInfo.ResultPriceCoins)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_coins);
            else if (offerInfo.Expired)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.offer_expired);
            else if (offerInfo.IsSpecial && !offerInfo.IsInfinite && await SpecialOfferUsersDao.NowUsed(User.Id, offerInfo.Id))
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.you_already_use_this_offer);
            else
            {
                User.Cash.Coins -= offerInfo.ResultPriceCoins;
                if (offerInfo.IsSpecial && !offerInfo.IsInfinite)
                    await SpecialOfferUsersDao.AddNew(User.Id, offerInfo.Id);
                var userPacks = await UsersPacksDao.GetUserPacks(User.Id);
                switch (offerInfo.PackId)
                {
                    case 1:
                        userPacks.RandomCount += offerInfo.Count;
                        break;
                    case 2:
                        userPacks.AuthorCount += offerInfo.Count;
                        break;
                    default:
                        var info = await SpecificPackDao.GetInfo(User.Id, offerInfo.PackId);
                        info.Count += offerInfo.Count;
                        break;
                }
                await MessageController.EditMessage(User, CallbackMessageId, Messages.thanks_for_buying);
                if (offerInfo.AdditionalPrize != "") await GivePrize(offerInfo.AdditionalPrize);
            }
        }

        private async Task GivePrize(string prizeInfo)
        {
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
                    sticker = await StickerDao.GetStickerByHash(data[1]);
                    break;
            }
            if (sticker != null)
            {
                if (!User.Stickers.ContainsKey(sticker.Md5Hash))
                    await UserStickerRelationDao.AddNew(User, sticker, 1);
                else
                {
                    await MessageController.AnswerCallbackQuery(User, CallbackQueryId,
                        $"{Messages.you_collected} {await User.Cash.Payout(User.Stickers)}{Text.coin}");
                    User.Stickers[sticker.Md5Hash].Count ++;
                }
                var stickerMessage = await MessageController.SendSticker(User, sticker.Id);
                var message = await MessageController.SendMessage(User, $"{Messages.congratulation}\n{sticker}");
                User.Session.Messages.Add(stickerMessage.MessageId);
                User.Session.Messages.Add(message.MessageId);
            }
        }

        public BuyByCoins() { }
        public BuyByCoins(UserEntity user, Update update) : base(user, update) { }
    }
}