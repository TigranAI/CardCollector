using System.Collections.Generic;
using CardCollector.Database.Entity;

namespace CardCollector.Session.Modules
{
    public class AdminModule : Module
    {
        public int? SelectedPackId;
        public long? SelectedStickerId;
        public List<Sticker> StickersList = new();
        public int? SelectedChannelGiveawayId;
        public int? ChatDistributionId;

        public void Reset()
        {
            SelectedPackId = null;
            SelectedStickerId = null;
            SelectedChannelGiveawayId = null;
            ChatDistributionId = null;
            StickersList.Clear();
        }
    }
}