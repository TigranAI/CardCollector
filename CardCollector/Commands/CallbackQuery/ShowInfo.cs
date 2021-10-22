using System.Globalization;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ShowInfo : CallbackQueryCommand
    {
        protected override string CommandText => Command.show_offer_info;
        public override async Task Execute()
        {
            var offerInfo = User.Session.GetModule<ShopModule>().SelectedPosition;
            var message = $"{offerInfo.Title}";
            if (offerInfo.Discount > 0) message += $"\n{Text.discount}: {offerInfo.Discount}%";
            if (offerInfo.AdditionalPrize != "") 
                message += $"\n{Text.prize}: {await PrizeToString(offerInfo.AdditionalPrize)}";
            var dateText = offerInfo.TimeLimited
                ? offerInfo.TimeLimit.ToString(CultureInfo.CurrentCulture).Split(' ')[0]
                : Text.unexpired;
            message += $"\n{Text.time_limit} {dateText}";
            if (offerInfo.Description != "") message += $"\n{Text.description}: {offerInfo.Description}";
            await MessageController.AnswerCallbackQuery(User, CallbackQueryId, message, true);
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

        public ShowInfo() { }
        public ShowInfo(UserEntity user, Update update) : base(user, update) { }
    }
}