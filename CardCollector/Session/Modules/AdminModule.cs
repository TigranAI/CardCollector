using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class AdminModule : Module
    {
        public Pack SelectedPack;
        public Sticker SelectedSticker;

        public void Reset()
        {
            SelectedPack = null;
            SelectedSticker = null;
        }
    }
}