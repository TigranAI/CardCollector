using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;

namespace CardCollector.Controllers 
{
    /* Контроллер аукциона, управляет всеми транзакциями
     между пользователями */
    public static class AuctionController 
    {
        /*Метод используется для продажи стикеров на аукционе
        user - пользователь, продающий стикер
        stickerShortHashCode - MD5 хеш представляющий собой сумму id стикера и id пользователя, используется в словаре как ключ
        price - цена за штуку
        count - количество продаваемых стикеров*/
        private static async Task SellCard(UserEntity user, int priceCoins, int priceGems)
        {
            /* Учитывая, что мы будем управлять кнопками + и -, то сюда никогда не поступят ложные данные
             решил метод упростить и задачу проверки переложил на взаимодействие с ботом 
             Пускай сам аукцион делает только то - что должен, проверки в сделку не входили
             Также в сессии пользователя теперь есть данные о выбранном текущем стикере, 
             так что можно не передавать хеш код  и количество */
            //подтверждаем действие
            var hash = user.Session.SelectedSticker.Md5Hash;
            user.Stickers[hash].Count -= user.Session.SelectedSticker.Count;
            /* Пока не думаю, что стоит сразу начислять сумму, пускай останется на будущее
            user.Cash.Coins += price * count;*/
            var product = new AuctionEntity
            {
                PriceCoins = priceCoins,
                PriceGems = priceGems,
                Quantity = user.Session.SelectedSticker.Count,
                StickerId = user.Session.SelectedSticker.Id,
                Trader = user.Id
            };
            // TODO реализовать добавление позиции на аукцион
            AuctionDao.AddNew(product);
        }
        
        //покупка стикера
        public static async Task BuyCard(StickerInfo sticker)
        {
            var product = await AuctionDao.GetProduct(sticker.TraderInfo.Id);
            product.Quantity -= sticker.Count;
            var user = await UserDao.GetById(product.Trader);
            var coinsSum = product.PriceCoins * sticker.Count;
            var gemsSum = product.PriceGems * sticker.Count;
            await MessageController.SendMessage(user, $"{Messages.you_sold} {sticker.Title} {sticker.Count}{Text.items}" +
                                                      $"\n{Messages.you_collected} {coinsSum}{Text.coin} / {gemsSum}{Text.gem}");
            user.Cash.Coins += coinsSum;
            user.Cash.Gems += gemsSum;
            if (product.Quantity == 0) AuctionDao.DeleteRow(sticker.TraderInfo.Id);
        }
        
        public static async Task<int> GetStickerCount(string stickerId)
        {
            return await AuctionDao.GetTotalQuantity(stickerId);
        }

        public static async Task<List<StickerEntity>> GetStickers(string filter)
        {
            return (await AuctionDao.GetStickers(filter)).ToList();
        }

        public static async Task<IEnumerable<TraderInformation>> GetTradersList(string filter, string stickerId)
        {
            var result = new List<TraderInformation>();
            var products = await AuctionDao.GetProducts(stickerId);
            var users = await UserDao.GetUsersList(filter);
            foreach (var product in products)
            {
                if (users.FirstOrDefault(i => i.Id == product.Trader) is { } user)
                    result.Add(new TraderInformation(product) {Username = user.Username});
            }
            return result;
        }

        public static async Task<int> GetStickerCount(int productId)
        {
            return await AuctionDao.GetQuantity(productId);
        }
    }
}