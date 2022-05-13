using System.Threading.Tasks;
using CardCollector.Cache.Entity;
using CardCollector.Cache.Repository;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

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
            Message message,
            User user)
        {
            if (chat.MembersCount + 1 < LADDER_MIN_USERS) return;

            var repo = new LadderInfoRepository();
            var info = await repo.GetOrDefaultAsync(message.Chat.Id, new LadderInfo());

            /*if (info!.IsLimitReached(LADDER_MAX_GAMES_PER_DAY)) return;
            //if (ladderInfo.IsLimitReached(chat.MaxLadderGames)) return;

            var sticker = await context.Stickers.FindByFileId(message.Sticker!.FileId);
            
            if (sticker.Tier == 10)
            {
                info.Reset();
                return;
            }
            var pack = sticker.Pack;

            info.SetPack(pack.Id);
            info.Add(user.Id, sticker.Id);

            if (info.TryComplete(LADDER_GOAL)) await SendPrizeMessage(chat, pack);

            await context.SaveChangesAsync();
            await repo.SaveAsync(message.Chat.Id, info);*/
        }

        private static async Task SendPrizeMessage(TelegramChat chat, Pack pack)
        {
            throw new System.NotImplementedException();
        }
    }
}