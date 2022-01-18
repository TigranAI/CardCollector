using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("messages")]
    public class UserMessages
    {
        [Key] [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("menu_id"), MaxLength(32)] public int MenuId { get; set; }
        [Column("collect_income_id"), MaxLength(32)] public int CollectIncomeId { get; set; }
        [Column("top_users_id"), MaxLength(32)] public int TopUsersId { get; set; }
        [Column("daily_task_id"), MaxLength(32)] public int DailyTaskId { get; set; }
        [Column("daily_task_progress_id"), MaxLength(32)] public int DailyTaskProgressId { get; set; }
    }
}