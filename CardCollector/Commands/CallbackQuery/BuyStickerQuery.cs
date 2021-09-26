using System;
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
            try
            {
                var selectedSticker = User.Session.SelectedSticker;
                var count = User.Session.State switch
                {
                    UserState.AuctionMenu => await AuctionController.GetStickerCount(selectedSticker.TraderInfo.Id),
                    UserState.ShopMenu => await ShopController.GetStickerCount(selectedSticker.Id),
                    _ => 0
                };
                var coinsPrice = selectedSticker.GetCoinsPrice();
                var gemsPrice = selectedSticker.GetGemsPrice();
                if (count < selectedSticker.Count && count != -1)
                {
                    await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id,
                        Messages.not_enougth_stickers);
                    await new BackToFiltersMenu(User, Update).Execute();
                }
                else if (coinsPrice > User.Cash.Coins)
                    await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id,
                        Messages.not_enougth_coins);
                else if (gemsPrice > User.Cash.Gems)
                    await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id,
                        Messages.not_enougth_gems);
                else
                {
                    switch (User.Session.State)
                    {
                        case UserState.AuctionMenu:
                            await AuctionController.BuyCard(selectedSticker);
                            break;
                        case UserState.ShopMenu:
                            await ShopController.SoldCard(selectedSticker);
                            break;
                    }

                    if (User.Stickers.ContainsKey(selectedSticker.Md5Hash))
                    {
                        await User.Session.PayOutOne(selectedSticker.Md5Hash);
                        await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id,
                            $"{Messages.you_collected} {User.Session.IncomeCoins}{Text.coin} / {User.Session.IncomeGems}{Text.gem}");
                    }

                    await UserStickerRelationDao.AddNew(User, selectedSticker, selectedSticker.Count);
                    User.Cash.Coins -= coinsPrice;
                    User.Cash.Gems -= gemsPrice;
                    User.Session.SelectedSticker = null;
                    await User.ClearChat();
                }
            }
            catch (Exception)
            {
                await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id, Messages.not_enougth_stickers);
                await new BackToFiltersMenu(User, Update).Execute();
            }
        }

        public BuyStickerQuery() { }
        public BuyStickerQuery(UserEntity user, Update update) : base(user, update) { }
    }
}