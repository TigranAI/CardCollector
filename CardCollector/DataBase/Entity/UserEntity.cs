using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("users")]
    public class UserEntity
    {
        [Column("id"), MaxLength(127)] public long Id { get; set; }
        [Column("chat_id"), MaxLength(127)] public long ChatId { get; set; }
        [Column("username"), MaxLength(256)] public string Username { get; set; }
        [Column("is_blocked"), MaxLength(11)] public bool IsBlocked { get; set; }
    }
}