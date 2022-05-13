using System.Collections.Generic;

namespace CardCollector.Cache.Entity
{
    public class LadderInfo
    {
        public int CurrentPackId = -1;
        public int GamesToday;
        public List<long> UserIds = new();
        public List<long> StickerIds = new();

        public void SetPack(int packId)
        {
            if (CurrentPackId != packId) Reset();
            CurrentPackId = packId;
        }

        public void Add(long userId, long stickerId)
        {
            if (UserIds.Contains(userId) || StickerIds.Contains(stickerId)) Reset();
            UserIds.Add(userId);
            StickerIds.Add(stickerId);
        }

        public bool TryComplete(int goal)
        {
            if (StickerIds.Count != goal) return false;
            GamesToday++;
            Reset();
            return true;
        }

        public bool IsLimitReached(int maxGames)
        {
            return GamesToday >= maxGames;
        }
        
        public void Reset()
        {
            UserIds.Clear();
            StickerIds.Clear();
            CurrentPackId = -1;
        }
    }
}