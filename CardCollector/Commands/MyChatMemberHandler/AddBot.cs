using System;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MyChatMemberHandler
{
    public class AddBot : MyChatMemberHandler
    {
        protected override Task Execute()
        {
            if (User.IsBlocked)
            {
                User.UnblockedAt = DateTime.Now;
                User.IsBlocked = false;
            }
            return Task.CompletedTask;
        }

        public override bool Match()
        {
            if (ChatMemberUpdated.NewChatMember.Status is not 
                (ChatMemberStatus.Member or ChatMemberStatus.Administrator)) return false;
            return ChatMemberUpdated.Chat.Type is not (ChatType.Group or ChatType.Supergroup or ChatType.Channel);
        }
    }
}