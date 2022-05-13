using System;
using System.Collections.Generic;
using System.Linq;
using CardCollector.Database.Entity.NotMapped;
using CardCollector.Resources.Translations;

namespace CardCollector.Database.Entity
{
    public class Cash
    {
        public int Coins { get; set; }
        public int Gems { get; set; }
        public int Candies { get; set; }
        public int MaxCapacity { get; set; } = 200;
        public DateTime LastPayout { get; set; } = DateTime.Now;

        public Income GetIncome(ICollection<UserSticker> stickers)
        {
            return new Income()
                .Calculate(stickers, LastPayout = DateTime.Now)
                .ApplyLimits(MaxCapacity, -1, stickers.Count(sticker => sticker.Sticker.Tier == 10));
        }

        public Income Payout(ICollection<UserSticker> stickers)
        {
            return new Income()
                .Calculate(stickers, LastPayout = DateTime.Now, true)
                .ApplyLimits(MaxCapacity, -1, stickers.Count(sticker => sticker.Sticker.Tier == 10))
                .Payout(this);
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