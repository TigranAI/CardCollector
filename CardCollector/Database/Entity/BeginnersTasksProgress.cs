using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Database.Entity
{
    public class BeginnersTasksProgress
    {
        public int Progress { get; set; }
        [NotMapped] public static int TaskCount = 12;
        
        public int SendStickersToPrivate { get; set; }
        [NotMapped] public static int SendStickersGoalToPrivate = 3;
        
        public bool CombineStickers { get; set; }
        
        public bool BuyStickerOnAuction { get; set; }
        
        public bool BuyStandardPack { get; set; }
        
        public bool OpenPack { get; set; }
        
        public bool TakePartAtChatGiveaway { get; set; }
        
        public int CollectIncome { get; set; }
        [NotMapped] public static int CollectIncomeGoal = 3;
        
        public int PlayRoulette { get; set; }
        [NotMapped] public static int PlayRouletteGoal = 6;
        
        public int WinRoulette { get; set; }
        [NotMapped] public static int WinRouletteGoal = 2;
        
        public bool PlaceStickerOnAuction { get; set; }
        
        public bool InviteFriend { get; set; }
        
        public bool Donate { get; set; }

        public int GetTasksProgress()
        {
            var result = 0;
            if (SendStickersToPrivate == SendStickersGoalToPrivate) result++;
            if (CombineStickers) result++;
            if (BuyStickerOnAuction) result++;
            if (BuyStandardPack) result++;
            if (OpenPack) result++;
            if (TakePartAtChatGiveaway) result++;
            if (CollectIncome == CollectIncomeGoal) result++;
            if (PlayRoulette == PlayRouletteGoal) result++;
            if (WinRoulette == WinRouletteGoal) result++;
            if (PlaceStickerOnAuction) result++;
            if (InviteFriend) result++;
            if (Donate) result++;
            return result;
        }
    }
}