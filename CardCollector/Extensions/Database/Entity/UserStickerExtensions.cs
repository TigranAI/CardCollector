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
            await userSticker.User.Messages.SendMessage(userSticker.User, 
                string.Format(Messages.congratulations_sticker_unlocked, userSticker.Sticker.Title));
        }
    }
}