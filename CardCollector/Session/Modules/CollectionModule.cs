namespace CardCollector.Session.Modules
{
    public class CollectionModule : Module
    {
        public long? SelectedStickerId;
        public int Count = 1;
        public int SellPrice;
        
        public void Reset()
        {
            SelectedStickerId = null;
            Count = 1;
            SellPrice = 0;
        }
    }
}