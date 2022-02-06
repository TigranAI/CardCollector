using System.Collections.Generic;
using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class AdminModule : Module
    {
        public int? SelectedPackId;
        public long? SelectedStickerId;
        public List<Sticker> StickersList = new();

        public void Reset()
        {
            SelectedPackId = null;
            SelectedStickerId = null;
            StickersList.Clear();
        }
    }
}