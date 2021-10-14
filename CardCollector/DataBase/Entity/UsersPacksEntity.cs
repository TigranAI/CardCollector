using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("users_packs")]
    public class UsersPacksEntity
    {
        /* Id пользователя */
        [Key] [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        
        /* Количество паков у пользователя */
        [Column("random_count"), MaxLength(32)] public int RandomCount { get; set; } = 0;
        [Column("author_count"), MaxLength(32)] public int AuthorCount { get; set; } = 0;
    }
}