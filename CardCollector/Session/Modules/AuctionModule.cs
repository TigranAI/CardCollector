using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class AuctionModule : Module
    {
        public Sticker SelectedSticker;
        public Auction SelectedPosition;
        public int Count = 1;
        public int MaxCount => SelectedPosition.Count;
        public int Price => SelectedPosition.Price;


        public void Reset()
        {
            SelectedSticker = null;
            SelectedPosition = null;
            Count = 0;
        }
    }
}