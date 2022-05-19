using System.Threading.Tasks;
using CardCollector.Cache.Repository;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Games
{
    public static class Giveaway
    {
        private static readonly int ACTIVITY_RATE = Constants.DEBUG ? 2 : 4;

        public static async Task OnMessageReceived(BotDatabaseContext context, TelegramChat chat)
        {
            var repo = new GiveawayInfoRepository();
            var info = await repo.GetAsync(chat);
            info.MessageCount++;

            if (info.TryComplete(chat, ACTIVITY_RATE))
            {
                var randomValue = Utilities.Rnd.Next(100);
                if (randomValue < 64)
                {
                    var stickers = await context.Stickers.FindAllByTier(1);
                    await SendRandomSticker(chat, stickers.Random());
                }
                else if (randomValue < 80)
                {
                    var stickers = await context.Stickers.FindAllByTier(2);
                    await SendRandomSticker(chat, stickers.Random());
                }
                else
                {
                    var pack = await context.Packs.FindById(1);
                    await SendRandomPack(chat, pack);
                }

                await SaveActivity(context, chat.Id);

                await AddToGiveawayList(chat.Id);
            }

            await repo.SaveAsync(chat, info);
        }

        private static async Task AddToGiveawayList(long chatId)
        {
            var listRepo = new ListRepository<long>();
            await listRepo.AddAsync(CallbackQueryCommands.group_claim_giveaway, chatId);
        }

        private static async Task SendRandomSticker(TelegramChat chat, Sticker sticker)
        {
            await chat.SendSticker(sticker.FileId);
            await chat.SendMessage(string.Format(Messages.time_to_claim, sticker),
                Keyboard.GroupClaimPrize(chat.Id, "sticker", sticker.Id));
        }

        private static async Task SendRandomPack(TelegramChat chat, Pack pack)
        {
            await chat.SendSticker(pack.PreviewFileId!);
            await chat.SendMessage(string.Format(Messages.time_to_claim, pack.Author),
                Keyboard.GroupClaimPrize(chat.Id, "pack", pack.Id));
        }

        private static async Task SaveActivity(BotDatabaseContext context, long chatId)
        {
            await context.UserActivities.AddAsync(new UserActivity()
            {
                Action = typeof(Giveaway).FullName,
                AdditionalData = $"chatId{chatId}"
            });
        }
    }
}