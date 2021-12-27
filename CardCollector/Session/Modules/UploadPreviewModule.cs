namespace CardCollector.Session.Modules
{
    public class UploadPreviewModule : Module
    {
        public string StickerId = "";
        public bool Animated = false;
        public int PackId = -1;

        public void Reset()
        {
            StickerId = "";
            Animated = false;
            PackId = -1;
        }
    }
}