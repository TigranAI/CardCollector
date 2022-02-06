using System.Collections.Generic;
using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class UploadedStickersModule : Module
    {
        public List<Sticker> StickersList = new();
        public int Count => StickersList.Count;
        
        public void Reset()
        {
            StickersList.Clear();
        }
    }
}