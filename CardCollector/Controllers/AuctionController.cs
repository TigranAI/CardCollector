using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
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
        private static async Task<ResultCode> SellCard(UserEntity user, string stickerShortHashCode, int price, int count = 1)
        {
            if (count > user.Stickers[stickerShortHashCode].Count)
                return ResultCode.NotEnoughStickers;
            //подтверждаем действие
            user.Stickers[stickerShortHashCode].Count -= count;
            user.Cash.Coins += price * count;
            return ResultCode.Ok;
        }
        
        private static async Task<ResultCode> BuyCard(UserEntity user, string stickerShortHashCode, int price, int count = 1)
        {
            if (user.Cash.Coins < count * price)
                return ResultCode.NotEnoughCash;
            //подтверждаем действие
            user.Stickers[stickerShortHashCode].Count += count;
            user.Cash.Coins += price * count;
            return ResultCode.Ok;
        }
        
        //TODO вернуть количество стикера на аукционе по его id
        public static async Task<int> GetStickerCount(string stickerId)
        {
            var count = await AuctionDao.GetProduct(stickerId);
            return count;
        }

        public static async Task<List<StickerEntity>> GetStickers(string filter)
        {
            //TODO вернуть список стикеров, имеющихся на аукционе
            return await StickerDao.GetAll(filter);
        }
    }
}