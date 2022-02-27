using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources.Translations;

namespace CardCollector.Database.Entity.NotMapped
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
            var sticker = stickers.Random();
            await user.AddSticker(context, sticker, 1);
            return $"\n{Text.random_sticker_added} {sticker.Title}";
        }

        private async Task<string> CheckRandomPacks(BotDatabaseContext context, User user)
        {
            if (RandomPacks is not { } packs) return "";
            var packInfo = await context.Packs.FindById(1);
            user.AddPack(packInfo, packs);
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