using System.Threading.Tasks;
using CardCollector.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MyChatMemberHandler
{
    public class KickFromChannel : MyChatMemberHandler
    {
        protected override async Task Execute()
        {
            var telegramChat = await Context.TelegramChats
                .SingleOrDefaultAsync(item => item.ChatId == ChatMemberUpdated.Chat.Id);
            if (telegramChat == null) return;
            telegramChat.IsBlocked = true;
        }

        public override bool Match()
        {
            if (ChatMemberUpdated.NewChatMember.Status is not 
                (ChatMemberStatus.Kicked or ChatMemberStatus.Left)) return false;
            return ChatMemberUpdated.Chat.Type is ChatType.Channel;
        }
    }
}