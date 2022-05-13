using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Games;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
                chat.MembersCount = await Bot.Client.GetChatMemberCountAsync(message.Chat.Id);
                
                if (!chat.Members.Contains(user)) chat.Members.Add(user);

                await Giveaway.OnMessageReceived(context, chat);
                
                if (message.Type is MessageType.Sticker && message.ViaBot?.Username == AppSettings.NAME)
                    await Ladder.OnStickerReceived(context, chat, message, user);

                await context.SaveChangesAsync();
            }
        }
    }
}