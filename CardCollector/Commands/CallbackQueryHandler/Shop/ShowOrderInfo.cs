using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class ShowOrderInfo : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.show_order_info;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            var specialOrderId = module.SelectedOrderId;
            var specialOrder = await Context.SpecialOrders.FindById(specialOrderId);
            if (specialOrder is null) return;
            var message = specialOrder.Title;
            if (specialOrder.Discount > 0) message += $"\n{Text.discount}: {specialOrder.Discount}%";
            if (specialOrder.AdditionalPrize is not null) message += $"\n{Text.prize}: {await PrizeToString(specialOrder.AdditionalPrize)}";
            message += $"\n{Text.time_limit} " +
                       $"{(specialOrder.TimeLimited ? specialOrder.TimeLimit.ToString()!.Split(' ')[0] : Text.unexpired)}";
            if (specialOrder.Description is not null) message += $"\n{Text.description}: {specialOrder.Description}";
            await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, message, true);
        }
        private async Task<string> PrizeToString(string prize)
        {
            var data = prize.Split('=');
            return data[0] switch
            {
                "tier" => $"{Text.sticker} {data[1]} {Text.tier2}",
                "sticker" => $"{Text.sticker} {(await Context.Stickers.FindById(long.Parse(data[1]))).Title}",
                _ => ""
            };
        }

        public override bool Match()
        {
            return base.Match() && User.Session.GetModule<ShopModule>().SelectedOrderId != null;
        }

        public ShowOrderInfo(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}