using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Database.Entity
{
    public class TelegramChat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ChatId { get; set; }
        public ChatType ChatType { get; set; }
        public string? Title { get; set; }
        public bool IsBlocked { get; set; }
        public bool DistributionsDisabled { get; set; }
        public int MaxExpGain { get; set; } = 20;
        public int GiveawayDuration { get; set; } = 180;

        public virtual ICollection<User> Members { get; set; } = new List<User>();
        public virtual ChatActivity? ChatActivity { get; set; }

        public InlineQueryResultArticle AsTelegramArticle(string command)
        {
            return new InlineQueryResultArticle($"{command}={Id}", Title ?? $"chat{Id}",
                new InputTextMessageContent(Text.select))
            {
                Description = $"{Text.members_count}: {Members.Count}"
            };
        }
    }
}