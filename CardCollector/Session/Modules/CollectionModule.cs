using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class CollectionModule : Module
    {
        public StickerEntity SelectedSticker;
        public int Count = 1;
        public int SellPrice = 0;
        
        public void Reset()
        {
            SelectedSticker = null;
            Count = 0;
            SellPrice = 0;
        }
    }
}