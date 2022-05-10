using System;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MyChatMemberHandler
{
    public class KickBot : MyChatMemberHandler
    {
        protected override Task Execute()
        {
            User.IsBlocked = true;
            User.BlockedAt = DateTime.Now;
            return Task.CompletedTask;
        }

        public override bool Match()
        {
            if (ChatMemberUpdated.NewChatMember.Status is not 
                (ChatMemberStatus.Kicked or ChatMemberStatus.Left)) return false;
            return ChatMemberUpdated.Chat.Type is not (ChatType.Group or ChatType.Supergroup or ChatType.Channel);
        }
    }
}