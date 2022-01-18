using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("special_offer_users")]
    public class SpecialOfferUsers
    {
        [Key] [Column("id"), MaxLength(127)] public long Id { get; set; }
        [Column("offer_id"), MaxLength(32)] public int OfferId { get; set; }
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("count"), MaxLength(32)] public int Count { get; set; } = 1;
    }
}