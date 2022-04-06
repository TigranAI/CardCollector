using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.Entity
{
    [Owned]
    public class BeginnersTasksProgress
    {
        public int SendStickersToPrivate { get; set; }
        [NotMapped] public int SentStickersGoalToPrivate = 3;
        
        public bool CombineStickers { get; set; }
        
        public bool BuyStickerOnAuction { get; set; }
        
        public bool BuyStandardPack { get; set; }
        
        public bool OpenPack { get; set; }
        
        public bool TakePartAtChatGiveaway { get; set; }
        
        public int ClaimIncome { get; set; }
        [NotMapped] public int ClaimIncomeGoal = 3;
        
        public int PlayRoulette { get; set; }
        [NotMapped] public int PlayRouletteGoal = 6;
        
        public int WinRoulette { get; set; }
        [NotMapped] public int WinRouletteGoal = 2;
        
        public bool PlaceStickerOnAuction { get; set; }
        
        public bool InviteFriend { get; set; }

        public int GetTasksProgress()
        {
            var result = 0;
            if (SendStickersToPrivate == SentStickersGoalToPrivate) result++;
            if (CombineStickers) result++;
            if (BuyStickerOnAuction) result++;
            if (BuyStandardPack) result++;
            if (OpenPack) result++;
            if (TakePartAtChatGiveaway) result++;
            if (ClaimIncome == ClaimIncomeGoal) result++;
            if (PlayRoulette == PlayRouletteGoal) result++;
            if (WinRoulette == WinRouletteGoal) result++;
            if (PlaceStickerOnAuction) result++;
            if (InviteFriend) result++;
            return result;
        }

        public int GetTaskCount()
        {
            return 11;
        }
    }
}