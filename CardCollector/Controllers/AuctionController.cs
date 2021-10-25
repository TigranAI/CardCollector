using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Session.Modules;

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
        public static void SellCard(long userId, string stickerId, int price, int count)
        {
            var product = new AuctionEntity
            {
                Trader = userId,
                StickerId = stickerId,
                Price = price,
                Count = count
            };
            AuctionDao.AddNew(product);
        }

        public static async Task<int> GetStickerCount(string stickerId)
        {
            return await AuctionDao.GetTotalQuantity(stickerId);
        }

        public static async Task<IEnumerable<StickerEntity>> GetStickers(string filter)
        {
            return await AuctionDao.GetStickers(filter);
        }

        public static async Task<IEnumerable<AuctionEntity>> GetTradersList(string filter, string stickerId)
        {
            return await AuctionDao.GetProducts(stickerId);
        }

        public static async Task<IEnumerable<int>> GetPriceList(string stickerId)
        {
            return (await AuctionDao.GetProducts(stickerId)).Select(item => item.Price);
        }

        public static async Task<int> GetStickerCount(int productId)
        {
            return await AuctionDao.GetQuantity(productId);
        }

        public static async Task<int> GetStickerCount(string stickerId, FiltersModule sessionFilters)
        {
            var traders = await GetTradersList("", stickerId);
            return sessionFilters.ApplyPriceTo(traders).Sum(i => i.Count);
        }
    }
}