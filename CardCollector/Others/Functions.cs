using System.Collections.Generic;
using System.Linq;
using CardCollector.Commands.CallbackQueryHandler.Collection;
using CardCollector.Commands.CallbackQueryHandler.Profile;
using CardCollector.Commands.ChosenInlineResultHandler.Group;
using CardCollector.Commands.ChosenInlineResultHandler.Private;
using CardCollector.Commands.MessageHandler.Shop;
using CardCollector.DataBase.Entity;

namespace CardCollector.Others
{
    public static class Functions
    {
        public static CountLogs CalculateActivityResults(CountLogs result, List<UserActivity> dayUsersActivities)
        {
            var groupedActivities = dayUsersActivities.GroupBy(item => item.User);

            result.PeopleCollectedIncomeMoreTimes = 0;
            result.PeopleCollectedIncomeOneToThreeTimes = 0;
            result.PeopleDonated = 0;
            result.PeopleCompletedDailyTask = 0;
            result.PeoplePutsStickerToAuction = 0;
            result.PeopleSendsStickerOneOrMoreTimes = 0;

            foreach (var userActivities in groupedActivities)
            {
                var userCollectIncomeTimes =
                    userActivities.Count(item => item.Action == typeof(CollectIncome).FullName);
                var isUserDonate =
                    userActivities.Any(item => item.Action == typeof(BuyGemsItem).FullName);
                var isUserCompleteDailyTask =
                    userActivities.Count(item => item.Action == typeof(SendPrivateSticker).FullName) >= 5;
                var isUserPutStickerToAuction =
                    userActivities.Any(item => item.Action == typeof(ConfirmSelling).FullName);
                var isUserSendStickerToChat =
                    userActivities.Any(item => item.Action == typeof(ChatSendSticker).FullName);

                if (userCollectIncomeTimes > 3) result.PeopleCollectedIncomeMoreTimes++;
                else if (userCollectIncomeTimes > 0) result.PeopleCollectedIncomeOneToThreeTimes++;
                if (isUserDonate) result.PeopleDonated++;
                if (isUserCompleteDailyTask) result.PeopleCompletedDailyTask++;
                if (isUserPutStickerToAuction) result.PeoplePutsStickerToAuction++;
                if (isUserSendStickerToChat) result.PeopleSendsStickerOneOrMoreTimes++;
            }

            return result;
        }
    }
}