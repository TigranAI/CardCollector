using System.Threading.Tasks;
using CardCollector.Attributes.Logs;
using CardCollector.Database;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MyChatMemberHandler
{
    [SavedActivity]
    public class AddToGroup : MyChatMemberHandler
    {
        protected override async Task Execute()
        {
            var telegramChat = await Context.TelegramChats
                .SingleOrDefaultAsync(item => item.ChatId == ChatMemberUpdated.Chat.Id);
            if (telegramChat == null)
            {
                telegramChat = (
                    await Context.TelegramChats.AddAsync(
                        new TelegramChat()
                        {
                            ChatId = ChatMemberUpdated.Chat.Id,
                            ChatType = ChatMemberUpdated.Chat.Type,
                            Title = ChatMemberUpdated.Chat.Title
                        }
                    )
                ).Entity;
            }

            telegramChat.IsBlocked = false;
        }

        public override bool Match()
        {
            if (ChatMemberUpdated.NewChatMember.Status is not 
                (ChatMemberStatus.Member or ChatMemberStatus.Administrator)) return false;
            return ChatMemberUpdated.Chat.Type is ChatType.Group or ChatType.Supergroup;
        }
    }
}