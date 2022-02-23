using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Bot.Types.Enums;

namespace CardCollector.DataBase.Entity
{
    public class TelegramChat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ChatId { get; set; }
        public ChatType ChatType { get; set; }
        public string? Title { get; set; }
        public bool IsBlocked { get; set; }
        
        public virtual ICollection<User> Members { get; set; }
        public virtual ChatActivity? ChatActivity { get; set; }
    }
}