using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Cache.Repository;
using CardCollector.Database;
using CardCollector.Extensions;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.TimerTasks
{
    public class ResetChatRestrictions : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG 
            ? new TimeSpan(DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes + Constants.TEST_ALERTS_INTERVAL, 0)
            : new TimeSpan(0, 0, 0);
        
        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            using (var context = new BotDatabaseContext())
            {
                await ResetChatsRestrictions(context);
                await ResetUsersRestrictions(context);
            }
        }

        private static async Task ResetUsersRestrictions(BotDatabaseContext context)
        {
            var userRepo = new UserInfoRepository();
            var users = await context.Users.Where(user => !user.IsBlocked).ToListAsync();
            await users.ApplyAsync(async user =>
            {
                var userInfo = await userRepo.GetAsync(user);
                userInfo.ResetRestrictions();
                await userRepo.SaveAsync(user, userInfo);
            });
        }

        private static async Task ResetChatsRestrictions(BotDatabaseContext context)
        {
            var chatRepo = new ChatInfoRepository();
            var ladderRepo = new LadderInfoRepository();
            var chats = await context.TelegramChats.Where(chat => !chat.IsBlocked).ToListAsync();
            await chats.ApplyAsync(async chat =>
            {
                var chatInfo = await chatRepo.GetAsync(chat);
                chatInfo.ResetRestrictions();
                await chatRepo.SaveAsync(chat, chatInfo);

                var ladderInfo = await ladderRepo.GetAsync(chat);
                ladderInfo.ResetRestrictions();
                await ladderRepo.SaveAsync(chat, ladderInfo);
            });
        }
    }
}