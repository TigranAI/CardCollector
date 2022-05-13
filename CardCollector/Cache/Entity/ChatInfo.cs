using System.Collections.Generic;

namespace CardCollector.Cache.Entity
{
    public class ChatInfo
    {
        public Dictionary<long, int> StickerCount = new();

        public bool IsLimitReached(long userId, int limit)
        {
            return StickerCount.TryGetValue(userId, out var count) && count >= limit;
        }

        public int GetAndIncrease(long userId)
        {
            if (StickerCount.TryGetValue(userId, out var count)) StickerCount[userId] = count + 1;
            else StickerCount.Add(userId, 1);
            return StickerCount[userId];
        }
    }
}