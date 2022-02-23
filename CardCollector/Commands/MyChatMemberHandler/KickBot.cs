using System.Threading.Tasks;
using CardCollector.DataBase;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MyChatMemberHandler
{
    public class KickBot : MyChatMemberHandler
    {
        protected override Task Execute()
        {
            User.IsBlocked = true;
            return Task.CompletedTask;
        }

        public override bool Match()
        {
            if (ChatMemberUpdated.NewChatMember.Status is not 
                (ChatMemberStatus.Kicked or ChatMemberStatus.Left)) return false;
            return ChatMemberUpdated.Chat.Type is not (ChatType.Group or ChatType.Supergroup or ChatType.Channel);
        }
        
        public KickBot(User user, BotDatabaseContext context, ChatMemberUpdated member) : base(user, context, member)
        {
        }
    }
}