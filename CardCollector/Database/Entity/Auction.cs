using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    public class Auction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public User Trader { get; set; }
        public Sticker Sticker { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}