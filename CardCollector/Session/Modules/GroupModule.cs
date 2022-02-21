namespace CardCollector.Session.Modules
{
    public class GroupModule : Module
    {
        public long? SelectBetChatId;
        public void Reset()
        {
            SelectBetChatId = null;
        }
    }
}