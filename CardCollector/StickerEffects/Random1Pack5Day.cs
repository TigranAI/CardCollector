using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;

namespace CardCollector.StickerEffects
{
    public static class Random1Pack5Day
    {
        public static int Interval = 5;

        public static async Task<int> GetPacksCount(Dictionary<string, UserStickerRelationEntity> stickers)
        {
            var today = DateTime.Today.ToString(CultureInfo.CurrentCulture);
            var stickersWithEffect = (await StickerDao.GetListWhere(
                item => item.Effect == (int) Effect.Random1Pack5Day)).Select(item => item.Md5Hash);
            var userStickers = stickers.Values.Where(item => stickersWithEffect.Contains(item.ShortHash));
            return userStickers.Sum(item =>
            {
                var interval = DateTime.Today - Convert.ToDateTime(item.AdditionalData);
                if (interval.Days < Interval) return 0;
                item.AdditionalData = today;
                return item.Count;
            });
        }
    }
}