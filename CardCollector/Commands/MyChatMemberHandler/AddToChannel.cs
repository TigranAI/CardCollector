using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MyChatMemberHandler
{
    public class AddToChannel : MyChatMemberHandler
    {
        protected override async Task Execute()
        {
            var telegramChat = await Context.TelegramChats.FindByChat(ChatMemberUpdated.Chat);
            telegramChat.IsBlocked = false;
        }

        public override bool Match()
        {
            if (ChatMemberUpdated.NewChatMember.Status is not
                (ChatMemberStatus.Member or ChatMemberStatus.Administrator)) return false;
            return ChatMemberUpdated.Chat.Type is ChatType.Channel;
        }
    }
}