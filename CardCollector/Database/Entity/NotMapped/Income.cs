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
                        if (updateTime) sticker.Payout = payoutTime;
                        Coins += sticker.GetIncome(payoutTime);
                        break;
                    case IncomeType.Gems:
                        if (updateTime) sticker.Payout = payoutTime;
                        Gems += sticker.GetIncome(payoutTime);
                        break;
                    case IncomeType.Candies when sticker.IsUnlocked:
                        if (updateTime) sticker.Payout = DateTime.Today;
                        Candies += sticker.GetIncome(payoutTime);
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
            user.Cash.Coins += Coins;
            await user.AddGems(Gems);
            user.Cash.Candies += Candies;
            return this;
        }

        public bool Empty()
        {
            return Coins == 0 && Gems == 0 && Candies == 0;
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