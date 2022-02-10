namespace CardCollector.Session.Modules
{
    public class AuctionModule : Module
    {
        public long? SelectedStickerId;
        public long? SelectedAuctionId;
        public int Count = 1;


        public void Reset()
        {
            SelectedStickerId = null;
            SelectedAuctionId = null;
            Count = 1;
        }
    }
}