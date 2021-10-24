using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("session_tokens")]
    public class SessionToken
    {
        [Key, Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("token"), MaxLength(256)] public string Token { get; set; }
    }
}