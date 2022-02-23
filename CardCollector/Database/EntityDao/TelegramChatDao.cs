using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace CardCollector.Database.EntityDao
{
    public static class TelegramChatDao
    {
        public static async Task<TelegramChat?> FindById(this DbSet<TelegramChat> table, int id)
        {
            return await table.SingleOrDefaultAsync(item => item.Id == id);
        }
        public static async Task<TelegramChat> FindChat(this DbSet<TelegramChat> table, Chat chat)
        {
            return await table.SingleOrDefaultAsync(item => item.ChatId == chat.Id)
                   ?? await table.Create(chat);
        }
        public static async Task<TelegramChat?> FindByChatId(this DbSet<TelegramChat> table, long chatId)
        {
            return await table.SingleOrDefaultAsync(item => item.ChatId == chatId);
        }
        public static async Task<TelegramChat> Create(this DbSet<TelegramChat> table, Chat chat)
        {
            var result = await table.AddAsync(new TelegramChat()
            {
                ChatId = chat.Id,
                ChatType = chat.Type,
                Title = chat.Title
            });
            return result.Entity;
        }
    }
}