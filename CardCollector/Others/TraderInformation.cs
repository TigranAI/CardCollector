using CardCollector.DataBase.Entity;

namespace CardCollector.Others
{
    public class TraderInformation : AuctionEntity
    {
        public TraderInformation(AuctionEntity entity)
        {
            Id = entity.Id;
            StickerId = entity.StickerId;
            PriceCoins = entity.PriceCoins;
            PriceGems = entity.PriceGems;
            Quantity = entity.Quantity;
            Trader = entity.Trader;
        }

        public string Username;
    }
}