using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class ShopModule : Module
    {
        public ShopEntity SelectedPosition;
        public void Reset()
        {
            SelectedPosition = null;
        }
    }
}