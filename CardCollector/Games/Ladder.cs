using System.Threading.Tasks;
using CardCollector.Cache.Repository;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Games
{
    public class Ladder
    {
        private static readonly int LADDER_MIN_USERS = Constants.DEBUG ? 2 : 20;
        private static readonly int LADDER_MAX_GAMES_PER_DAY = Constants.DEBUG ? 5 : 3;
        private static readonly int LADDER_GOAL = Constants.DEBUG ? 2 : 7;

        public static async Task OnStickerReceived(
            BotDatabaseContext context,
            TelegramChat chat,
            Sticker sticker,
            User user)
        {
            if (chat.MembersCount + 1 < LADDER_MIN_USERS) return;

            var repo = new LadderInfoRepository();
            var info = await repo.GetAsync(chat);

            if (info.IsLimitReached(LADDER_MAX_GAMES_PER_DAY)) return;
            //if (ladderInfo.IsLimitReached(chat.MaxLadderGames)) return;
            
            if (sticker.Tier == 10)
            {
                info.Reset();
                return;
            }
            var pack = sticker.Pack;

            info.Add(user.Id, pack.Id, sticker.Id);

            if (await info.TryComplete(LADDER_GOAL))
            {
                await SendPrizeMessage(chat, pack, info.GamesToday);
                await SaveActivity(context, chat.Id);
                await AddToLadderList(chat.Id);
            }

            await repo.SaveAsync(chat, info);
        }

        private static async Task SendPrizeMessage(TelegramChat chat, Pack pack, int gamesToday)
        {
            if (pack.PreviewFileId != null) await chat.SendSticker(pack.PreviewFileId);
            await chat.SendMessage(string.Format(Messages.ladder_message,
                    LADDER_GOAL, pack.Author, gamesToday, LADDER_MAX_GAMES_PER_DAY),
                Keyboard(chat.Id, pack.Id));
        }

        private static InlineKeyboardMarkup Keyboard(long chatId, int packId)
        {
            return new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.claim, 
                    $"{CallbackQueryCommands.group_claim_ladder}={chatId}={packId}") 
            });
        }
        
        private static async Task AddToLadderList(long chatId)
        {
            var listRepo = new ListRepository<long>();
            await listRepo.AddAsync(CallbackQueryCommands.group_claim_ladder, chatId);
        }
        
        private static async Task SaveActivity(BotDatabaseContext context, long chatId)
        {
            await context.UserActivities.AddAsync(new UserActivity()
            {
                Action = typeof(Ladder).FullName,
                AdditionalData = $"chatId{chatId}"
            });
        }
    }
}