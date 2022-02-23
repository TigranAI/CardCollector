using System.Threading.Tasks;
using CardCollector.Database;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MyChatMemberHandler
{
    public class AddBot : MyChatMemberHandler
    {
        protected override Task Execute()
        {
            User.IsBlocked = false;
            return Task.CompletedTask;
        }

        public override bool Match()
        {
            if (ChatMemberUpdated.NewChatMember.Status is not 
                (ChatMemberStatus.Member or ChatMemberStatus.Administrator)) return false;
            return ChatMemberUpdated.Chat.Type is not (ChatType.Group or ChatType.Supergroup or ChatType.Channel);
        }
        
        public AddBot(User user, BotDatabaseContext context, ChatMemberUpdated member) : base(user, context, member)
        {
        }
    }
}