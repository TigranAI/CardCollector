using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("count_logs")]
    public class CountLogs
    {
        [Key, Column("date"), MaxLength(32)] public DateTime Date { get; set; }
        [Column("pciottt"), MaxLength(32)] public int PeopleCollectedIncomeOneToThreeTimes { get; set; }
        [Column("pcimt"), MaxLength(32)] public int PeopleCollectedIncomeMoreTimes { get; set; }
        [Column("pcdt"), MaxLength(32)] public int PeopleCompletedDailyTask { get; set; }
        [Column("pssoomt"), MaxLength(32)] public int PeopleSendsStickerOneOrMoreTimes { get; set; }
        [Column("pd"), MaxLength(32)] public int PeopleDonated { get; set; }
        [Column("ppsta"), MaxLength(32)] public int PeoplePutsStickerToAuction { get; set; }
    }
}