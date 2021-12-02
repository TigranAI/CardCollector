using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("messages")]
    public class UserMessages
    {
        [Key] [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("menu_id"), MaxLength(32)] public int MenuId { get; set; } = -1;
        [Column("collect_income_id"), MaxLength(32)] public int CollectIncomeId { get; set; } = -1;
        [Column("top_users_id"), MaxLength(32)] public int TopUsersId { get; set; } = -1;
        [Column("daily_task_id"), MaxLength(32)] public int DailyTaskId { get; set; } = -1;
        [Column("daily_task_progress_id"), MaxLength(32)] public int DailyTaskProgressId { get; set; } = -1;
    }
}