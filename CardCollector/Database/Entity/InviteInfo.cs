using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.UrlCommands;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.Entity
{
    public class InviteInfo
    {
        [NotMapped] private static IEnumerable<char> alphabet = 
            "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ1234567890";
        [NotMapped] private const int MAX_ATTEMPTS = 50;

        [Key, ForeignKey("id")]
        public virtual User User { get; set; }
        public string? InviteKey { get; set; }
        
        public virtual User? InvitedBy { get; set; }
        public DateTime? InvitedAt { get; set; }
        public virtual ICollection<User> InvitedFriends { get; set; }

        public virtual BeginnersTasksProgress? TasksProgress { get; set; }

        public string GetTelegramUrl()
        {
            return string.Format(Text.telegram_url_pattern, AppSettings.NAME, MessageUrlCommands.invite_friend, InviteKey);
        }

        public bool ShowInvitedBy()
        {
            return InvitedBy != null &&
                   DateTime.Now - InvitedAt <= Constants.BEGINNERS_TASKS_INTERVAL;
        }

        public static async Task<string> GenerateKey()
        {
            using (var context = new BotDatabaseContext())
            {
                return await Generate(context);
            }
        }

        private static async Task<string> Generate(BotDatabaseContext context)
        {
            var result = GenerateRandomString();
            if (!await context.InviteInfo.AnyAsync(item => result == item.InviteKey)) return result;
            
            for (var i = 0; i < MAX_ATTEMPTS; ++i)
            {
                result = GenerateRandomString();
                if (!await context.InviteInfo.AnyAsync(item => result == item.InviteKey)) return result;
            }

            for (var i = 8;; ++i)
            {
                result = GenerateRandomString(i);
                if (!await context.InviteInfo.AnyAsync(item => result == item.InviteKey)) return result;
            }
        }

        private static string GenerateRandomString(int length = 7)
        {
            var sb = new StringBuilder();
            length.Times(() => sb.Append(alphabet.Random()));
            return sb.ToString();
        }
    }
}