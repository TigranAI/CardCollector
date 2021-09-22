using CardCollector.DataBase.Entity;

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
        
        public int count;
    }
}