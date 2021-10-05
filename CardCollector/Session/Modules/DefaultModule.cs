using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class DefaultModule : Module
    {
        public StickerEntity SelectedSticker;
        public void Reset()
        {
            SelectedSticker = null;
        }
    }
}