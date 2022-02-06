using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.DataBase.Entity.NotMapped
{
    public class LevelReward
    {
        public int? CashCapacity = null;
        public int? RandomPacks = null;
        public int? RandomStickerTier = null;

        public async Task<string> GetReward(BotDatabaseContext context, User user)
        {
            var rewardText = CheckCashCapacity(user);
            rewardText += await CheckRandomPacks(context, user);
            rewardText += await CheckRandomStickerTier(context, user);
            return rewardText;
        }

        private async Task<string> CheckRandomStickerTier(BotDatabaseContext context, User user)
        {
            if (RandomStickerTier is not { } tier) return "";
            
            var stickers = await context.Stickers.FindAllByTier(tier);
            var sticker = stickers[Utilities.rnd.Next(stickers.Length)];
            if (user.Stickers.SingleOrDefault(item => item.Sticker.Id == sticker.Id) is { } userSticker)
                userSticker.Count++;
            else
                user.Stickers.Add(new UserSticker()
                {
                    User = user,
                    Sticker = sticker,
                    Count = 1
                });

            return $"\n{Text.random_sticker_added} {sticker.Title}";
        }

        private async Task<string> CheckRandomPacks(BotDatabaseContext context, User user)
        {
            if (RandomPacks is not { } packs) return "";

            var pack = user.Packs.SingleOrDefault(item => item.Id == 1);
            if (pack != null) pack.Count += packs;
            else
            {
                var packInfo = await context.Packs.FindPack(1);
                user.Packs.Add(new UserPacks(user, packInfo, packs));
            }

            return $"\n{Text.random_packs_added} {packs}{Text.items}";
        }

        private string CheckCashCapacity(User user)
        {
            if (CashCapacity is not { } capacity) return "";

            user.Cash.MaxCapacity += capacity;

            return $"\n{Text.cash_capacity_increased} +{capacity}{Text.coin}";
        }
    }
}