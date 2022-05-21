using System;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Resources.Translations;

namespace CardCollector.Extensions.Database.Entity
{
    public static class UserStickerExtensions
    {
        public static async Task DoExclusiveTask(this UserSticker userSticker, int modifier = 1)
        {
            if (userSticker.IsUnlocked) return;
            userSticker.ExclusiveTaskProgress += modifier;
            if (userSticker.ExclusiveTaskProgress < userSticker.Sticker.ExclusiveTaskGoal) return;
            userSticker.IsUnlocked = true;
            userSticker.Payout = DateTime.Today.AddDays(-1);
            await userSticker.User.Messages.SendMessage( 
                string.Format(Messages.congratulations_sticker_unlocked, userSticker.Sticker.Title));
        }

        public static int GetIncome(this UserSticker userSticker, DateTime payoutTime)
        {
            var interval = payoutTime - userSticker.Payout;
            var payoutsCount = (int) (interval.TotalMinutes / userSticker.Sticker.IncomeTime);
            return payoutsCount > 0
                ? payoutsCount * userSticker.Sticker.Income * userSticker.Count
                : 0;
        }
    }
}