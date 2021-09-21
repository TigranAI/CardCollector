using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;

namespace CardCollector.Controllers
{
    public static class ShopController
    {
        public static async Task<int> GetStickerCount(string stickerId)
        {
            //TODO вернуть количество стикеров в магазине по id
            return 0;
        }
        
        public static async Task<List<StickerEntity>> GetStickers(string filter)
        {
            //TODO вернуть список стикеров, имеющихся в магазине
            return await StickerDao.GetAll(filter);
        }
    }
}