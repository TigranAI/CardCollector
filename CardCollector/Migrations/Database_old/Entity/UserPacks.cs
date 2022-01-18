using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("users_packs")]
    public class UserPacks
    {
        [Key] [Column("id"), MaxLength(127)] public long Id { get; set; }
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("pack_id"), MaxLength(32)] public int PackId { get; set; }
        [Column("count"), MaxLength(32)] public int Count { get; set; }
    }
}