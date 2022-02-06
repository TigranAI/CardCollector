using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CardCollector.DailyTasks;

namespace CardCollector.DataBase.Entity
{
    public class DailyTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public User User { get; set; }
        public DailyTaskKeys TaskId { get; set; }
        public int Progress { get; set; }
    }
}