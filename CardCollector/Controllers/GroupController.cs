using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Games;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramUser = Telegram.Bot.Types.User;

namespace CardCollector.Controllers
{
    public static class GroupController
    {
        public static async Task OnMessageReceived(Message message)
        {
            using (var context = new BotDatabaseContext())
            {
                var chat = await context.TelegramChats.FindByChat(message.Chat);
                if (chat.IsBlocked) return;
                
                var user = await context.Users.FindUser(message.From!);
                await context.SaveChangesAsync();
                chat.MembersCount = await Bot.Client.GetChatMemberCountAsync(message.Chat.Id);
                
                if (!chat.Members.Contains(user)) chat.Members.Add(user);

                await Giveaway.OnMessageReceived(context, chat);

                await context.SaveChangesAsync();
            }
        }
    }
}