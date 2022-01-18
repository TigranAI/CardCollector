using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("cash")]
    public class CashEntity
    {
        [Key]
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("coins"), MaxLength(32)] public int Coins { get; set; }
        [Column("gems"), MaxLength(32)] public int Gems { get; set; }
        [Column("max_capacity"), MaxLength(32)] public int MaxCapacity { get; set; }
    }
}