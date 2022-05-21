using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Translations;

namespace CardCollector.Others
{
    public static class BeginnersTasksProgressRewards
    {
        public static Dictionary<int, Func<BotDatabaseContext, User, User, Task>> RewardMap = new()
        {
            {1, PackReward},
            {2, PackReward},
            {3, PackReward},
            {4, PackReward},
            {5, PackReward},
            {6, PackAndTier1StickerRewardWithInviter},
            {7, ExpReward},
            {8, GemsReward},
            {9, GemsRewardWithInviter},
            {10, Tier2StickerReward},
            {11, Tier3StickerRewardWithInviter},
            {12, Tier3StickerReward}
        };
        
        private static async Task PackReward(BotDatabaseContext context, User inviter, User invited)
        {
            var pack = await context.Packs.FindById(1);
            invited.AddPack(pack, 1);
            await invited.Messages.SendMessage(string.Format(Messages.beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                Messages.beginners_task_pack_reward));
        }

        private static async Task PackAndTier1StickerRewardWithInviter(BotDatabaseContext context, User inviter, User invited)
        {
            await invited.Messages.SendMessage(string.Format(Messages.beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                Messages.beginners_task_pack_and_tier2_sticker_reward));
            
            var pack = await context.Packs.FindById(1);
            invited.AddPack(pack, 1);
            var stickers = await context.Stickers.FindAllByTier(2);
            await invited.AddSticker(stickers.Random(), 1, true);

            await inviter.Messages.SendMessage(string.Format(Messages.friend_beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                invited.Username, Messages.beginners_task_gems_reward));

            await inviter.AddGems(100);
        }

        private static async Task ExpReward(BotDatabaseContext context, User inviter, User invited)
        {
            await invited.Messages.SendMessage(string.Format(Messages.beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                Messages.beginners_task_exp_reward));
            
            invited.Level.GiveExp(1000);
            await invited.Level.CheckLevelUp(context);
        }

        private static async Task GemsReward(BotDatabaseContext context, User inviter, User invited)
        {
            await invited.Messages.SendMessage(string.Format(Messages.beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                Messages.beginners_task_gems_reward));

            await invited.AddGems(100);
        }

        private static async Task GemsRewardWithInviter(BotDatabaseContext context, User inviter, User invited)
        {
            await invited.Messages.SendMessage(string.Format(Messages.beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                Messages.beginners_task_gems_reward));

            await invited.AddGems(100);
            
            await inviter.Messages.SendMessage(string.Format(Messages.friend_beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                invited.Username, Messages.beginners_task_tier2_sticker_reward));
            
            var stickers = await context.Stickers.FindAllByTier(2);
            await inviter.AddSticker(stickers.Random(), 1, true);
        }

        private static async Task Tier2StickerReward(BotDatabaseContext context, User inviter, User invited)
        {
            await invited.Messages.SendMessage(string.Format(Messages.beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                Messages.beginners_task_tier2_sticker_reward));
            
            var stickers = await context.Stickers.FindAllByTier(2);
            await invited.AddSticker(stickers.Random(), 1, true);
        }

        private static async Task Tier3StickerRewardWithInviter(BotDatabaseContext context, User inviter, User invited)
        {
            await invited.Messages.SendMessage(string.Format(Messages.beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                Messages.beginners_task_tier2_sticker_reward + " x3"));
            
            var stickers = await context.Stickers.FindAllByTier(2);
            await invited.AddSticker(stickers.Random(), 1, true);
            await invited.AddSticker(stickers.Random(), 1, true);
            await invited.AddSticker(stickers.Random(), 1, true);
            
            await inviter.Messages.SendMessage(string.Format(Messages.friend_beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                invited.Username, Messages.beginners_task_tier3_sticker_reward));
            
            stickers = await context.Stickers.FindAllByTier(3);
            await inviter.AddSticker(stickers.Random(), 1, true);
        }

        private static async Task Tier3StickerReward(BotDatabaseContext context, User inviter, User invited)
        {
            await invited.Messages.SendMessage(string.Format(Messages.beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                Messages.beginners_task_tier3_sticker_reward));
            
            var stickers = await context.Stickers.FindAllByTier(3);
            await invited.AddSticker(stickers.Random(), 1, true);
            
            /*await inviter.Messages.SendMessage(string.Format(Messages.friend_beginners_task_reward,
                invited.InviteInfo!.TasksProgress!.Progress, BeginnersTasksProgress.TaskCount,
                invited.Username, Messages.beginners_task_tier4_sticker_reward));
            
            await inviter.AddSticker(stickers.Random(), 1, true);*/
        }
    }
}