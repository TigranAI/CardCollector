using System;
using System.Collections.Generic;
using System.Linq;

namespace CardCollector.DataBase.Entity
{
    public class Cash
    {
        public int Coins { get; set; }
        public int Gems { get; set; }
        public int MaxCapacity { get; set; } = 200;
        public DateTime LastPayout { get; set; } = DateTime.Now;
        
        public int GetIncome(ICollection<UserSticker> stickers)
        {
            LastPayout = DateTime.Now;
            var result = stickers.Sum(sticker => Payout(sticker));
            return result > MaxCapacity ? MaxCapacity : result;
        }
        
        public int Payout(ICollection<UserSticker> stickers)
        {
            var result = stickers.Sum(sticker => Payout(sticker, true));
            result = result > MaxCapacity ? MaxCapacity : result;
            Coins += result;
            return result;
        }
        
        private int Payout(UserSticker userSticker, bool updatePayout = false)
        {
            var payoutInterval = LastPayout - userSticker.Payout;
            var payoutsCount = (int) (payoutInterval.TotalMinutes / userSticker.Sticker.IncomeTime);
            if (updatePayout) userSticker.Payout = LastPayout;
            if (payoutsCount < 1) return 0;
            return userSticker.Sticker.Income * payoutsCount * userSticker.Count;
        }
    }
}