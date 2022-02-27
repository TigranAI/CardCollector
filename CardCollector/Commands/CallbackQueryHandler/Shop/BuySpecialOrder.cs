using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Sticker = CardCollector.Database.Entity.Sticker;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class BuySpecialOrder : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.buy_shop_item;

        protected override async Task Execute()
        {
            var orderId = User.Session.GetModule<ShopModule>().SelectedOrderId;
            var orderInfo = await Context.SpecialOrders.FindById(orderId);
            if (orderInfo is null) return;

            var currency = CallbackQuery.Data!.Split('=')[1];
            if (currency == "coins" && User.Cash.Coins < orderInfo.GetResultPriceCoins())
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_coins, true);
            else if (currency == "gems" && User.Cash.Gems < orderInfo.GetResultPriceGems())
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_gems, true);
            else if (orderInfo.IsExpired())
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.order_expired, true);
            else if (!orderInfo.IsInfinite && User.SpecialOrdersUser.Any(item => item.Order.Id == orderInfo.Id))
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.you_already_use_this_order,
                    true);
            else
            {
                await User.Messages.ClearChat(User);
                if (currency == "coins") User.Cash.Coins -= orderInfo.GetResultPriceCoins();
                else if (currency == "gems") User.Cash.Gems -= orderInfo.GetResultPriceGems();
                if (!orderInfo.IsInfinite)
                    User.SpecialOrdersUser.Add(new SpecialOrderUser()
                    {
                        User = User,
                        Order = orderInfo
                    });
                var packInfo = await Context.Packs.FindById(orderInfo.Pack.Id);
                User.AddPack(packInfo, orderInfo.Count);
                var userPack = User.Packs.Single(item => item.Pack.Id == orderInfo.Pack.Id);
                await GivePrize(orderInfo.AdditionalPrize);
                await User.Messages.EditMessage(User,
                    $"{Messages.thanks_for_buying} {userPack.Count} {Text.packs} {packInfo.Author}",
                    orderInfo.IsExpired() || !orderInfo.IsInfinite
                        ? Keyboard.BackShopKeyboard
                        : Keyboard.BuyShopItem(CallbackQuery.Data));
            }
        }

        private async Task GivePrize(string? prizeInfo)
        {
            if (prizeInfo is null) return;
            var data = prizeInfo.Split('=');
            Sticker? sticker = null;
            switch (data[0])
            {
                case "tier":
                    var tier = int.Parse(data[1]);
                    var stickers = await Context.Stickers.FindAllByTier(tier);
                    sticker = stickers.Random();
                    break;
                case "sticker":
                    sticker = await Context.Stickers.FindById(long.Parse(data[1]));
                    break;
            }

            if (sticker is not null)
            {
                await User.AddSticker(Context, sticker, 1);
                await User.Messages.ClearChat(User);
                await User.Messages.SendSticker(User, sticker.FileId);
                await User.Messages.SendMessage(User, $"{Messages.congratulation}\n{sticker}");
            }
        }

        public override bool Match()
        {
            return base.Match() && User.Session.GetModule<ShopModule>().SelectedOrderId != null;
        }

        public BuySpecialOrder(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}