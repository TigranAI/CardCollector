using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class BuyShopPack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.buy_shop_item;

        protected override async Task Execute()
        {
            var packId = User.Session.GetModule<ShopModule>().SelectedPackId;
            var packInfo = await Context.Packs.FindById(packId);

            var currency = CallbackQuery.Data!.Split('=')[1];
            if (currency == "coins" && User.Cash.Coins < packInfo.PriceCoins)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_coins, true);
            else if (currency == "gems" && User.Cash.Gems < packInfo.PriceGems)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_gems, true);
            else
            {
                await User.Messages.ClearChat(User);
                if (currency == "coins") User.Cash.Coins -= packInfo.PriceCoins;
                else if (currency == "gems") User.Cash.Gems -= packInfo.PriceGems;
                User.AddPack(packInfo, 1);
                var userPack = User.Packs.Single(item => item.Pack.Id == packInfo.Id);
                await User.Messages.EditMessage(User,
                    $"{Messages.thanks_for_buying} {userPack.Count} {Text.packs} {packInfo.Author}",
                    Keyboard.BuyShopItem(CallbackQuery.Data));
            }
        }

        public override bool Match()
        {
            return base.Match() && User.Session.GetModule<ShopModule>().SelectedPackId != null;
        }

        public BuyShopPack(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}