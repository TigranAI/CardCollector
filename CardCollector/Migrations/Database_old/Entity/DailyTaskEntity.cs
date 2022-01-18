using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("daily_task")]
    public class DailyTaskEntity
    {
        [Column("id"), MaxLength(127)] public long Id { get; set; }
        [Column("task_id"), MaxLength(32)] public int TaskId { get; set; }
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("progress"), MaxLength(32)] public int Progress { get; set; }
    }
}