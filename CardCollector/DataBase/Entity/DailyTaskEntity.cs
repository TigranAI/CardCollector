using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("daily_task")]
    public class DailyTaskEntity
    {
        /* Id записи в таблице, роли не играет */
        [Column("id"), MaxLength(127)] public long Id { get; set; }
        
        /* Id задачи */
        [Column("task_id"), MaxLength(32)] public int TaskId { get; set; }
        
        /* Id пользователя */
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        
        /* Прогресс пользователя по задаче */
        [Column("progress"), MaxLength(32)] public int Progress { get; set; }
    }
}