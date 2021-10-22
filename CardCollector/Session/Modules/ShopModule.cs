using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class ShopModule : Module
    {
        public ShopEntity SelectedPosition;
        public int EnteredExchangeSum = 1;
        public void Reset()
        {
            SelectedPosition = null;
        }
    }
}