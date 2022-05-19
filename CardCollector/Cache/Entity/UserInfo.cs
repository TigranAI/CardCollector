namespace CardCollector.Cache.Entity
{
    public class UserInfo
    {
        public int LadderPrizes;
        public long PuzzleChatId;

        public bool TryClaimLadder(int limit)
        {
            if (LadderPrizes == limit) return false;
            LadderPrizes++;
            return true;
        }

        public void ResetRestrictions()
        {
            LadderPrizes = 0;
        }
    }
}