namespace CardCollector.Session.Modules
{
    public class ShopModule : Module
    {
        
        public int? SelectedPackId;
        public int? SelectedOrderId;
        public int Count = 1;
        public int EnteredExchangeSum = 1;
        public string? SelectedProvider;
        
        public void Reset()
        {
            SelectedPackId = null;
            SelectedOrderId = null;
            Count = 1;
            EnteredExchangeSum = 1;
            SelectedProvider = null;
        }
    }
}