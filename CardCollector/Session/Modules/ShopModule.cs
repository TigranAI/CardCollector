using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class ShopModule : Module
    {
        public ShopEntity SelectedPosition;
        public PackEntity SelectedPack;
        public int Count = 1;
        public int EnteredExchangeSum = 1;
        public void Reset()
        {
            SelectedPosition = null;
            SelectedPack = null;
            Count = 1;
            EnteredExchangeSum = 1;
        }
    }
}