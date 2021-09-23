using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using Telegram.Bot.Types.InlineQueryResults;

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
        private static async Task BuyCard(UserEntity user, int price, int count = 1)
        {
            // TODO @darkgolly попробуй переписать метод аналогично тому, что выше
            var hash = user.Session.SelectedSticker.Md5Hash;
            user.Stickers[hash].Count += user.Session.SelectedSticker.Count;
            user.Cash.Coins -= user.Session.IncomeCoins;
            user.Cash.Gems -= user.Session.IncomeGems;
            
            AuctionDao.SoldStikers(hash);
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
    }
}