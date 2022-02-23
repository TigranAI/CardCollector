using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Resources;

namespace CardCollector.StickerEffects
{
    public class RandomSticker2Tier3Day
    {
        public static int Interval = 3;

        /*public static async Task<int> GetStickersCount(Dictionary<string, UserSticker> stickers)
        {
            var today = DateTime.Today.ToString(Constants.TimeCulture.ShortDatePattern);
            var stickersWithEffect = (await StickerDao.GetListWhere(
                item => item.Effect == (int) Effect.RandomSticker2Tier3Day)).Select(item => item.Md5Hash);
            var userStickers = stickers.Values.Where(item => stickersWithEffect.Contains(item.ShortHash));
            return userStickers.Sum(item =>
            {
                var interval = DateTime.Today - Convert.ToDateTime(item.AdditionalData, Constants.TimeCulture);
                if (interval.Days < Interval) return 0;
                item.AdditionalData = today;
                return item.Count;
            });
        }

        public static async Task<Dictionary<Sticker, int>> GenerateList(int count)
        {
            var stickers = await StickerDao.GetListWhere(item => item.Tier == 2);
            var rnd = new Random();
            var result = new List<Sticker>();
            for (var i = 0; i < count; i++)
                result.Add(stickers[rnd.Next(stickers.Count)]);
            return result.GroupBy(item => item).ToDictionary(item => item.Key, item => item.Count());
        }*/
    }
}