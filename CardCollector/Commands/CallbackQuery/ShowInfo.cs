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
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            var description = module.SelectedPosition?.Description ?? module.SelectedPack?.Description ?? "";
            var message = module.SelectedPosition?.Title ?? $"{Messages.author_pack}: {module.SelectedPack?.Author}";
            if (module.SelectedPosition?.Discount > 0) message += $"\n{Text.discount}: {module.SelectedPosition?.Discount}%";
            if (module.SelectedPosition != null && module.SelectedPosition?.AdditionalPrize != "") 
                message += $"\n{Text.prize}: {await PrizeToString(module.SelectedPosition?.AdditionalPrize)}";
            var dateText = module.SelectedPosition?.TimeLimited ?? false
                ? module.SelectedPosition?.TimeLimit.ToString(CultureInfo.CurrentCulture).Split(' ')[0]
                : Text.unexpired;
            if (module.SelectedPosition != null) message += $"\n{Text.time_limit} {dateText}";
            if (module.SelectedPack != null) message += $"\n{Text.opened_count} {module.SelectedPack.OpenedCount}{Text.items}";
            if (description != "") message += $"\n{Text.description}: {description}";
            await MessageController.AnswerCallbackQuery(User, CallbackQueryId, message, true);
        }
        private async Task<string> PrizeToString(string prize)
        {
            var data = prize.Split('=');
            return data[0] switch
            {
                "tier" => $"{Text.sticker} {data[1]} {Text.tier2}",
                "sticker" => $"{Text.sticker} {(await StickerDao.GetByHash(data[1])).Title}",
                _ => ""
            };
        }

        public ShowInfo() { }
        public ShowInfo(UserEntity user, Update update) : base(user, update) { }
    }
}