using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity.NotMapped;
using CardCollector.Resources.Translations;

namespace CardCollector.Database.Entity
{
    [Table("user_cash")]
    public class Cash
    {
        [Key, ForeignKey("id")]
        public virtual User User { get; set; }
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

        public async Task<Income> Payout()
        {
            return await new Income()
                .Calculate(User.Stickers, LastPayout = DateTime.Now, true)
                .ApplyLimits(MaxCapacity, -1, User.Stickers.Count(sticker => sticker.Sticker.Tier == 10))
                .Payout(User);
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