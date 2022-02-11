using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    public class Auction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual User Trader { get; set; }
        public virtual Sticker Sticker { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}