using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class DefaultModule : Module
    {
        public Sticker SelectedSticker;
        public void Reset()
        {
            SelectedSticker = null;
        }
    }
}