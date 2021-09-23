using System.Linq;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;

namespace CardCollector.Others
{
    /* Класс-расщирение, который может содержать дополнительную информацию о стикере */
    public class StickerInfo : StickerEntity
    {
        public StickerInfo(StickerEntity entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Author = entity.Author;
            IncomeCoins = entity.IncomeCoins;
            IncomeGems = entity.IncomeGems;
            IncomeTime = entity.IncomeTime;
            PriceCoins = entity.PriceCoins;
            PriceGems = entity.PriceGems;
            Tier = entity.Tier;
            Emoji = entity.Emoji;
            Description = entity.Description;
            Md5Hash = entity.Md5Hash;
        }
        
        public int Count = 1;
        public int MaxCount;
        
        
        public TraderInformation TraderInfo = null;

        public int GetCoinsPrice()
        {
            return Count * TraderInfo?.PriceCoins ?? PriceCoins;
        }
        
        public int GetGemsPrice()
        {
            return Count * TraderInfo?.PriceGems ?? PriceGems;
        }

        public override string ToString()
        {
            var count = TraderInfo?.Quantity ?? MaxCount;
            var str = $"\n{Title} {string.Concat(Enumerable.Repeat(Text.star, Tier))}" +
                $"\n{Text.emoji}: {Emoji}" +
                $"\n{Text.author}: {Author}" +
                $"\n{Text.count}: {(count != -1 ? count : "∞")}" +
                $"\n{IncomeCoins}{Text.coin} / {IncomeGems}{Text.gem} {IncomeTime}{Text.time}{Text.minutes}";
            if (Description != "") str += $"\n\n{Text.description}: {Description}";
            return str;
        }
    }
}