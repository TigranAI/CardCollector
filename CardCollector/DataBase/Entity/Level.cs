using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.DataBase.Entity
{
    [Table("levels")]
    public class Level
    {
        [Key] [Column("id"), MaxLength(32)] public int Id { get; set; }
        [Column("level_value"), MaxLength(32)] public int LevelValue { get; set; } = 0;
        [Column("level_exp_goal"), MaxLength(127)] public long LevelExpGoal { get; set; } = 0;
        [Column("level_reward"), MaxLength(512)] public string JSONLevelReward { get; set; } = "{}";

        public LevelReward GetRewardInstance()
        {
            try {
                return Utilities.FromJson<LevelReward>(JSONLevelReward);
            } catch (Exception e) {
                Logs.LogOutError(e);
                return null;
            }
        }

        [NotMapped]
        public class LevelReward
        {
            public int? CashCapacity = null;
            public int? RandomPacks = null;
            public int? RandomStickerTier = null;

            public async Task<string> GetReward(UserEntity user)
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
                    rewardText += $"\n{Text.random_sticker_added}\n{sticker}";
                }
                return rewardText;
            }
        }
    }
}