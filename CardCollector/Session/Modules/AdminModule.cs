using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class AdminModule : Module
    {
        public PackEntity SelectedPack;
        public StickerEntity SelectedSticker;

        public void Reset()
        {
            SelectedPack = null;
            SelectedSticker = null;
        }
    }
}