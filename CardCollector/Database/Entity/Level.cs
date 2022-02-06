using System.ComponentModel.DataAnnotations;
using CardCollector.DataBase.Entity.NotMapped;

namespace CardCollector.DataBase.Entity
{
    public class Level
    {
        [Key] public int Id { get; set; }
        public int LevelValue { get; set; }
        public long LevelExpGoal { get; set; }
        public LevelReward LevelReward { get; set; } = new ();
    }
}