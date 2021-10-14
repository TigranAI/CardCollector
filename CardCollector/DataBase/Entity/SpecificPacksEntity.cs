using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("specific_packs")]
    public class SpecificPacksEntity
    {
        [Key][Column("id"), MaxLength(127)] public long Id { get; set; }
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("pack_id"), MaxLength(32)] public int PackId { get; set; }
        [Column("count"), MaxLength(32)] public int Count { get; set; }
    }
}