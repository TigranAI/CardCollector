using System;
using System.ComponentModel.DataAnnotations;

namespace CardCollector.DataBase.Entity
{
    public class CountLogs
    {
        [Key] public DateTime Date { get; set; } = DateTime.Today;
        public int PeopleCollectedIncomeOneToThreeTimes { get; set; }
        public int PeopleCollectedIncomeMoreTimes { get; set; }
        public int PeopleCompletedDailyTask { get; set; }
        public int PeopleSendsStickerOneOrMoreTimes { get; set; }
        public int PeopleDonated { get; set; }
        public int PeoplePutsStickerToAuction { get; set; }
        public int GroupCountWasAdded { get; set; }
        public int GroupCountWasActive { get; set; }
        public int RoulettePlayCount { get; set; }
        public int GroupPrizeCount { get; set; }

        public bool IsEmpty()
        {
            return PeopleCollectedIncomeOneToThreeTimes == 0
                   && PeopleCollectedIncomeMoreTimes == 0
                   && PeopleCompletedDailyTask == 0
                   && PeopleSendsStickerOneOrMoreTimes == 0
                   && PeopleDonated == 0
                   && PeoplePutsStickerToAuction == 0
                   && GroupCountWasAdded == 0
                   && GroupCountWasActive == 0
                   && RoulettePlayCount == 0
                   && GroupPrizeCount == 0;
        }
    }
}