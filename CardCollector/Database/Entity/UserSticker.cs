using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Database.Entity
{
    public class UserSticker
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual User User { get; set; }
        public virtual Sticker Sticker { get; set; }
        public int Count { get; set; }
        public DateTime Payout { get; set; } = DateTime.Now;
        public DateTime GivePrizeDate { get; set; } = DateTime.Today;
        public DateTime LastUsage { get; set; } = DateTime.Now;
    }
}