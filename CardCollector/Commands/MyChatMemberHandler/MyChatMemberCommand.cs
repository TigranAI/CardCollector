using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MyChatMemberHandler
{
    /* Родительский класс для входящих обновлений типа MyChatMember
     (Добавление/Добавление в чаты/Добавление в каналы/Блокировки/Исключения бота)
     Данный класс полностью реализован и не нуждается в наследовании */
    public class MyChatMemberCommand : HandlerModel
    {
        protected override string CommandText => "";

        private readonly ChatMemberUpdated _chatMemberUpdated;
        protected override Task Execute()
        {
            var status = _chatMemberUpdated.NewChatMember.Status;
            switch (status)
            {
                case ChatMemberStatus.Member:
                    User.IsBlocked = false;
                    break;
                case ChatMemberStatus.Kicked:
                    User.IsBlocked = true;
                    break;
                case ChatMemberStatus.Restricted or ChatMemberStatus.Left:
                    break;
            }
            return Task.CompletedTask;
        }

        public override bool Match() => true;

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.MyChatMember!.From);
            
            user.InitSession();

            return new MyChatMemberCommand(user, context, update.MyChatMember);
        }

        private MyChatMemberCommand(User user, BotDatabaseContext context, ChatMemberUpdated member) : base(user, context)
        {
            _chatMemberUpdated = member;
        }
    }
}