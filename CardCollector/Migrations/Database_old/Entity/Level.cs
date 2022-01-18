using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("levels")]
    public class Level
    {
        [Key] [Column("id"), MaxLength(32)] public int Id { get; set; }
        [Column("level_value"), MaxLength(32)] public int LevelValue { get; set; }
        [Column("level_exp_goal"), MaxLength(127)] public long LevelExpGoal { get; set; }
        [Column("level_reward"), MaxLength(512)] public string? JSONLevelReward { get; set; }
    }
}