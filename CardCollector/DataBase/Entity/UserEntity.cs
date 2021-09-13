using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.DataBase.Entity
{
    [Table("users")]
    public class UserEntity
    {
        [Key]
        [Column("id"), MaxLength(127)] public long Id { get; set; }
        [Column("chat_id"), MaxLength(127)] public long ChatId { get; set; }
        [Column("username"), MaxLength(256)] public string Username { get; set; }
        [Column("is_blocked"), MaxLength(11)] public bool IsBlocked { get; set; }
        
        [NotMapped] public CashEntity Cash { get; set; }
        
        public async Task<IEnumerable<InlineQueryResult>> GetStickersList(string command, string filter)
        {
            var result = new List<InlineQueryResult>();
            var stickers = await UserStickerRelationDao.GetListById(Id);
            foreach (var sticker in stickers.Values)
            {
                if (filter != "")
                {
                    var stickerInfo = await StickerDao.GetStickerInfo(sticker.StickerId);
                    if (!stickerInfo.Title.Contains(filter)) break;
                }
                var item = new InlineQueryResultCachedSticker($"{command}={sticker.ShortHash}", sticker.StickerId);
                result.Add(item);
                if (result.Count > 49) return result;
            }
            return result;
        }
    }
}