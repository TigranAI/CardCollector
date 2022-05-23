using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Extensions;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;

namespace CardCollector.Database.Entity.NotMapped
{
    public class Income
    {
        public int Coins;
        public int Gems;
        public int Candies;

        public Income Calculate(ICollection<UserSticker> stickers, DateTime payoutTime, bool updateTime = false)
        {
            stickers.Apply(sticker =>
            {
                switch (sticker.Sticker.IncomeType)
                {
                    case IncomeType.Coins:
                        var coinIncome = sticker.GetIncome(payoutTime);
                        if (coinIncome <= 0) break;
                        if (updateTime) sticker.Payout = payoutTime;
                        Coins += coinIncome;
                        break;
                    case IncomeType.Gems:
                        var gemIncome = sticker.GetIncome(payoutTime);
                        if (gemIncome <= 0) break;
                        if (updateTime) sticker.Payout = payoutTime;
                        Gems += gemIncome;
                        break;
                    case IncomeType.Candies when sticker.IsUnlocked:
                        var candyIncome = sticker.GetIncome(payoutTime);
                        if (candyIncome <= 0) break;
                        if (updateTime) sticker.Payout = DateTime.Today;
                        Candies += candyIncome;
                        break;
                }
            });
            return this;
        }

        public Income ApplyLimits(int coinsCapacity, int gemsCapacity, int candiesCapacity)
        {
            Coins = Math.Min(coinsCapacity, Coins);
            Gems = Math.Min(gemsCapacity, Gems);
            Candies = Math.Min(candiesCapacity, Candies);
            return this;
        }

        public async Task<Income> Payout(User user)
        {
            if (Coins > 0) user.Cash.Coins += Coins;
            if (Gems > 0) await user.AddGems(Gems);
            if (Candies > 0) user.Cash.Candies += Candies;
            return this;
        }

        public bool Empty()
        {
            return Coins <= 0 && Gems <= 0 && Candies <= 0;
        }

        public override string ToString()
        {
            List<string> values = new();
            if (Coins > 0) values.Add($"{Coins}{Text.coin}");
            if (Gems > 0) values.Add($"{Gems}{Text.gem}");
            if (Candies > 0) values.Add($"{Candies}{Text.candy}");
            return string.Join(" ", values);
        }
    }
}