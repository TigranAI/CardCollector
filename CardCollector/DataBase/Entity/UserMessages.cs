using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CardCollector.DataBase.Entity
{
    [Table("messages")]
    public class UserMessages
    {
        /* id записи */
        [Key] [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        /* id стикера */
        [Column("menu_id"), MaxLength(32)] public int MenuId { get; set; } = -1;

        public async Task SendMenu()
        {
            try
            {
                if (MenuId != -1)
                    await Bot.Client.DeleteMessageAsync(UserId, MenuId);
            } catch { /**/ }
            try
            {
                var msg = await Bot.Client.SendTextMessageAsync(UserId, Messages.main_menu, replyMarkup: Keyboard.Menu,
                    parseMode: ParseMode.Html);
                MenuId = msg.MessageId;
            } catch { /**/ }
        }
    }
}