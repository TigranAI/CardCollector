using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("user_level")]
    public class UserLevel
    {
        [Key] [Column("id"), MaxLength(127)] public long UserId { get; set; }
        [Column("level"), MaxLength(32)] public int Level { get; set; }
        [Column("current_exp"), MaxLength(127)] public long CurrentExp { get; set; }
        [Column("total_exp"), MaxLength(127)] public long TotalExp { get; set; }
    }
}