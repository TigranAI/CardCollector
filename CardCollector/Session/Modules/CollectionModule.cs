using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class CollectionModule : Module
    {
        public Sticker SelectedSticker;
        public int Count = 1;
        public int SellPrice = 0;
        
        public void Reset()
        {
            SelectedSticker = null;
            Count = 1;
            SellPrice = 0;
        }
    }
}