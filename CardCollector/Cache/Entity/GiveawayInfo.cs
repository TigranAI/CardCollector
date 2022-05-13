using System;
using CardCollector.Database.Entity;

namespace CardCollector.Cache.Entity
{
    public class GiveawayInfo
    {
        public int MessageCount;

        public void Reset()
        {
            MessageCount = 0;
        }

        public bool TryComplete(TelegramChat chat, int activityRate)
        {
            if (chat.LastGiveaway != null)
            {
                var interval = DateTime.Now - chat.LastGiveaway!.Value;
                if (interval.TotalMinutes < chat.GiveawayDuration) return false;
            }
            if (chat.MembersCount * activityRate > MessageCount) return false;
            
            Reset();
            chat.LastGiveaway = DateTime.Now;
            return true;
        }
    }
}