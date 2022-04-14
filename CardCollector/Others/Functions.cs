using System.Collections.Generic;
using System.Linq;
using CardCollector.Commands.CallbackQueryHandler.Collection;
using CardCollector.Commands.CallbackQueryHandler.Profile;
using CardCollector.Commands.ChosenInlineResultHandler.Group;
using CardCollector.Commands.ChosenInlineResultHandler.Private;
using CardCollector.Commands.MessageHandler.Group;
using CardCollector.Commands.MessageHandler.Shop;
using CardCollector.Commands.MessageHandler.UrlCommands;
using CardCollector.Commands.MyChatMemberHandler;
using CardCollector.Controllers;
using CardCollector.Database.Entity;

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
            result.GroupCountWasAdded = 0;
            result.GroupCountWasActive = 0;
            result.RoulettePlayCount = 0;
            result.GroupPrizeCount = 0;
            result.InvitedUsers = 0;

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
                var groupsAddedCount =
                    userActivities.Count(item => item.Action == typeof(AddToGroup).FullName);
                var rouletteCount =
                    userActivities.Count(item => item.Action == typeof(Roulette).FullName);

                var groupsWithPrize =
                    userActivities.Where(item => item.Action == typeof(GroupController).FullName);
                var groupsActiveCount = groupsWithPrize
                    .DistinctBy(item => item.AdditionalData)
                    .Count();
                var groupPrizeCount = groupsWithPrize.Count();

                var invitedUsers = userActivities.Count(item => item.Action == typeof(Invite).FullName);

                if (userCollectIncomeTimes > 3) result.PeopleCollectedIncomeMoreTimes++;
                else if (userCollectIncomeTimes > 0) result.PeopleCollectedIncomeOneToThreeTimes++;
                if (isUserDonate) result.PeopleDonated++;
                if (isUserCompleteDailyTask) result.PeopleCompletedDailyTask++;
                if (isUserPutStickerToAuction) result.PeoplePutsStickerToAuction++;
                if (isUserSendStickerToChat) result.PeopleSendsStickerOneOrMoreTimes++;
                
                result.GroupCountWasAdded += groupsAddedCount;
                result.GroupCountWasActive += groupsActiveCount;
                result.RoulettePlayCount += rouletteCount;
                result.GroupPrizeCount += groupPrizeCount;
                result.InvitedUsers += invitedUsers;
            }

            return result;
        }
    }
}