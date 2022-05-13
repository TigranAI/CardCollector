namespace CardCollector.Cache.Entity
{
    public class UserInfo
    {
        public int ladderPrizes;

        public bool TryClaimLadder(int limit)
        {
            if (ladderPrizes == limit) return false;
            ladderPrizes++;
            return true;
        }
    }
}