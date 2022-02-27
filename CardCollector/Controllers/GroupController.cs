using System;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramUser = Telegram.Bot.Types.User;

namespace CardCollector.Controllers
{
    public static class GroupController
    {
        public static readonly int GROUP_GIVEAWAY_HOURS_INTERVAL = Constants.DEBUG ? 1 : 8;
        private static readonly int ACTIVITY_RATE = Constants.DEBUG ? 5 : 4;

        public static async Task OnGroupMessageReceived(Chat chat, TelegramUser user)
        {
            using (var context = new BotDatabaseContext())
            {
                var telegramChat = await context.TelegramChats.FindByChatId(chat.Id);
                if (telegramChat is null) telegramChat = await context.TelegramChats.Create(chat);
                if (telegramChat.ChatActivity == null) telegramChat.ChatActivity = new ChatActivity();
                telegramChat.ChatActivity.MessageCount++;

                var botUser = await context.Users.FindUser(user);
                if (!telegramChat.Members.Contains(botUser)) telegramChat.Members.Add(botUser);

                await CheckActivityGiveaway(context, telegramChat);

                await context.SaveChangesAsync();
            }
        }

        private static async Task CheckActivityGiveaway(BotDatabaseContext context, TelegramChat chat)
        {
            if (chat.ChatActivity!.LastGiveaway != null)
            {
                var interval = DateTime.Now - chat.ChatActivity.LastGiveaway.Value;
                if (interval.TotalHours < GROUP_GIVEAWAY_HOURS_INTERVAL) return;
            }
            var membersCount = await Bot.Client.GetChatMemberCountAsync(chat.ChatId) - 1;
            var messageCount = chat.ChatActivity.MessageCount - chat.ChatActivity.MessageCountAtLastGiveaway;
            if (messageCount < membersCount * ACTIVITY_RATE) return;
            await SendGiveaway(context, chat);
        }

        private static async Task SendGiveaway(BotDatabaseContext context, TelegramChat chat)
        {
            if(chat.ChatActivity.GiveawayAvailable) return;
            chat.ChatActivity.GiveawayAvailable = true;
            await context.UserActivities.AddAsync(new UserActivity()
            {
                Action = typeof(GroupController).FullName,
                AdditionalData = $"chatId{chat.Id}"
            });
            chat.ChatActivity!.PrizeClaimed = false;
            var randomValue = Utilities.rnd.Next(100);
            if (randomValue < 64) await SendRandomSticker(context, chat, 1);
            else if (randomValue < 80) await SendRandomSticker(context, chat, 2);
            else await SendRandomPack(context, chat);
        }

        private static async Task SendRandomSticker(BotDatabaseContext context, TelegramChat chat, int tier)
        {
            var stickers = await context.Stickers.FindAllByTier(tier);
            var sticker = stickers.Random();
            await chat.SendSticker(sticker.FileId);
            await chat.SendMessage(string.Format(Messages.time_to_claim, sticker),
                Keyboard.GroupClaimPrize(chat.Id, "sticker", sticker.Id));
        }

        private static async Task SendRandomPack(BotDatabaseContext context, TelegramChat chat)
        {
            var pack = await context.Packs.FindById(1);
            await chat.SendSticker(pack.PreviewFileId!);
            await chat.SendMessage(string.Format(Messages.time_to_claim, pack.Author),
                Keyboard.GroupClaimPrize(chat.Id, "pack", pack.Id));
        }
    }
}