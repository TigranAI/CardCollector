using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;

namespace CardCollector.Controllers
{
    public static class ShopController
    {
        public static async Task<int> GetStickerCount(string stickerId)
        {
            var sticker = await ShopDao.GetSticker(stickerId);
            return sticker.IsInfinite ? -1 : sticker.Count;
        }
        
        public static async Task<List<StickerEntity>> GetStickers(string filter)
        {
            var result = await ShopDao.GetAllShopPositions(filter);
            return result.ToList();
        }

        public static async Task SoldCard(StickerInfo selectedSticker)
        {
            var product = await ShopDao.GetSticker(selectedSticker.Id);
            if (product.IsInfinite) return;
            product.Count -= selectedSticker.Count;
            if (product.Count == 0) ShopDao.DeleteRow(product.Id);
        }
    }
}