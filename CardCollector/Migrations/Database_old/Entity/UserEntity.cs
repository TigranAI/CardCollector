using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("users")]
    public class UserEntity
    {
        [Key]
        [Column("id"), MaxLength(127)] public long Id { get; set; }
        [Column("chat_id"), MaxLength(127)] public long ChatId { get; set; }
        [Column("username"), MaxLength(256)] public string Username { get; set; }
        [Column("is_blocked"), MaxLength(11)] public bool IsBlocked { get; set; }
        [Column("privilege_level"), MaxLength(32)] public int _privilegeLevel { get; set; }
        [Column("first_reward"), MaxLength(11)] public bool FirstReward { get; set; }
    }
}