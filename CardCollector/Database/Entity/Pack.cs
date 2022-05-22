using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using Telegram.Bot.Types;

namespace CardCollector.Database.Entity
{
    public class Pack
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(127)] public string Author { get; set; }
        [MaxLength(512)] public string? Description { get; set; }
        public int PriceCoins { get; set; }
        public int PriceGems { get; set; }
        [MaxLength(127)] public string? PreviewFileId { get; set; }
        public bool? IsPreviewAnimated { get; set; }
        public string? GifPreviewFileId { get; set; }
        public long OpenedCount { get; set; }
        public bool IsExclusive { get; set; } = false;
        public virtual ICollection<Sticker> Stickers { get; set; }

        public Sticker OpenExclusive(ICollection<UserSticker> userStickers)
        {
            var availableStickers = userStickers
                .Where(us => us.Sticker.Pack.Id == Id)
                .Select(us => us.Sticker)
                .ToList();
            return availableStickers.Count == Stickers.Count
                ? Stickers.Random()
                : Stickers.Except(availableStickers).Random();
        }

        public async Task<Sticker> Open()
        {
            var tier = GetTier(Utilities.Rnd.NextDouble() * 100);
            if (Id != 1) return Stickers.Where(sticker => sticker.Tier == tier).Random();
            using (var context = new BotDatabaseContext())
            {
                var stickers = await context.Stickers.FindAllByTier(tier);
                return stickers.Where(sticker => sticker.ExclusiveTask == ExclusiveTask.None).Random();
            }
        }

        private static int GetTier(double chance)
        {
            return chance switch
            {
                <= 0.7 => 4,
                <= 3.3 => 3,
                <= 16 => 2,
                _ => 1
            };
        }

        public void SetPreview(Telegram.Bot.Types.Sticker sticker)
        {
            PreviewFileId = sticker.FileId;
            IsPreviewAnimated = sticker.IsAnimated;
        }

        public void SetGifPreview(File file)
        {
            GifPreviewFileId = file.FileId;
        }
    }
}