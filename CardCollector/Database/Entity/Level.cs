using System.ComponentModel.DataAnnotations;
using CardCollector.Database.Entity.NotMapped;

namespace CardCollector.Database.Entity
{
    public class Level
    {
        [Key] public int Id { get; set; }
        public int LevelValue { get; set; }
        public long LevelExpGoal { get; set; }
        public LevelReward LevelReward { get; set; } = new ();
    }
}