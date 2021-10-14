using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectOfferCallback : CallbackQuery
    {
        protected override string CommandText => Command.select_offer;
        public override async Task Execute()
        {
            var offerId = int.Parse(CallbackData.Split('=')[1]);
            var offerInfo = await ShopDao.GetById(offerId);
            var packInfo = await PacksDao.GetById(offerInfo.PackId);
            User.Session.GetModule<ShopModule>().SelectedPosition = offerInfo;
            var message = $"{offerInfo.Title}" +
                          $"\n{packInfo.Author}" +
                          $"{(packInfo.Description != "" ? $"\n{packInfo.Description}" : "")}";
            if (offerInfo.Discount > 0) message += $"\n{Text.discount}: {offerInfo.Discount}%";
            if (offerInfo.Count > 1) message += $"\n{Text.count}: {offerInfo.Count}{Text.items}";
            if (offerInfo.AdditionalPrize != "") 
                message += $"\n{Text.prize}: {await PrizeToString(offerInfo.AdditionalPrize)}";
            if (offerInfo.TimeLimited) message += $"\n{Text.time_limit}: {offerInfo.TimeLimit}";
            await MessageController.EditMessage(User, CallbackMessageId, message, Keyboard.OfferKeyboard(offerInfo));
        }

        private async Task<string> PrizeToString(string prize)
        {
            var data = prize.Split('=');
            return data[0] switch
            {
                "tier" => $"{Text.sticker} {data[1]} {Text.tier2}",
                "sticker" => $"{Text.sticker} {(await StickerDao.GetStickerByHash(data[1])).Title}",
                _ => ""
            };
        }

        public SelectOfferCallback() { }
        public SelectOfferCallback(UserEntity user, Update update) : base(user, update) { }
    }
}