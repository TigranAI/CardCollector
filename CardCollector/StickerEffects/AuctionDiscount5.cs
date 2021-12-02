using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;

namespace CardCollector.StickerEffects
{
    public static class AuctionDiscount5
    {
        public static async Task<bool> IsApplied(Dictionary<string, UserStickerRelation> stickers)
        {
            return await stickers.AnyAsync(async item =>
            {
                var stickerInfo = await StickerDao.GetById(item.Value.StickerId);
                return stickerInfo.Effect == (int) Effect.AuctionDiscount5 && item.Value.Count != 0;
            });
        }
    }
}