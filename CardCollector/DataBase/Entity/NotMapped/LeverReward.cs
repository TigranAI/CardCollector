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

        /*public async Task<string> GetReward(User user)
        {
            var rewardText = "";
            if (CashCapacity is { } capacity)
            {
                user.Cash.MaxCapacity += capacity;
                rewardText += $"\n{Text.cash_capacity_increased} +{capacity}{Text.coin}";
            }
            if (RandomPacks is { } packs)
            {
                var userPack = await UserPacksDao.GetOne(user.Id, 1);
                userPack.Count += packs;
                rewardText += $"\n{Text.random_packs_added} {packs}{Text.items}";
            }
            if (RandomStickerTier is { } tier)
            {
                var stickers = await StickerDao.GetListWhere(item => item.Tier == tier);
                var sticker = stickers[Utilities.rnd.Next(stickers.Count)];
                await UserStickerRelationDao.AddSticker(user, sticker);
                rewardText += $"\n{Text.random_sticker_added} {sticker.Title}";
            }
            return rewardText;
        }*/
    }
}