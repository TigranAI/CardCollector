using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("cash")]
    public class CashEntity
    {
        [Key]
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("coins"), MaxLength(32)] public int Coins { get; set; } = 0;
        [Column("gems"), MaxLength(32)] public int Gems { get; set; } = 0;
    }
}