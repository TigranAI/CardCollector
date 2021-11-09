using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("count_logs")]
    public class CountLogs
    {
        [Key, Column("date"), MaxLength(32)] public DateTime Date { get; set; } = DateTime.Today;
        [Column("pciottt"), MaxLength(32)] public int PeopleCollectedIncomeOneToThreeTimes { get; set; } = 0;
        [Column("pcimt"), MaxLength(32)] public int PeopleCollectedIncomeMoreTimes { get; set; } = 0;
        [Column("pcdt"), MaxLength(32)] public int PeopleCompletedDailyTask { get; set; } = 0;
        [Column("pssoomt"), MaxLength(32)] public int PeopleSendsStickerOneOrMoreTimes { get; set; } = 0;
        [Column("pd"), MaxLength(32)] public int PeopleDonated { get; set; } = 0;
        [Column("ppsta"), MaxLength(32)] public int PeoplePutsStickerToAuction { get; set; } = 0;
    }
}