namespace CardCollector.Session.Modules
{
    public class DefaultModule : Module
    {
        public long? SelectedStickerId;
        public void Reset()
        {
            SelectedStickerId = null;
        }
    }
}