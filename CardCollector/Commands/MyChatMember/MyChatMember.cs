using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MyChatMember
{
    public class MyChatMember : UpdateModel
    {
        protected override string Command => "";
        private readonly ChatMemberStatus _status;
        public override async Task Execute()
        {
            switch (_status)
            {
                case ChatMemberStatus.Creator:
                    await UserDao.GetUser(ChatToUser(Update.MyChatMember!.Chat));
                    break;
                case ChatMemberStatus.Administrator:
                    await UserDao.GetUser(ChatToUser(Update.MyChatMember!.Chat));
                    break;
                case ChatMemberStatus.Member:
                    User.IsBlocked = false;
                    break;
                case ChatMemberStatus.Kicked:
                    User.IsBlocked = false;
                    break;
                case ChatMemberStatus.Restricted or ChatMemberStatus.Left:
                    break;
                default:
                    await new CommandNotFound(User, Update, _status.ToString()).Execute();
                    break;
            }
        }
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Объект пользователя
            var user = await UserDao.GetUser(update.MyChatMember!.From);
            return new MyChatMember(user, update, update.MyChatMember.NewChatMember.Status);
        }

        private static User ChatToUser(Chat chat)
        {
            return new User
            {
                Username = chat.Username,
                FirstName = chat.FirstName ?? chat.Title ?? "",
                LastName = chat.LastName ?? "",
                Id = chat.Id,
                IsBot = chat.Id < 0
            };
        }

        private MyChatMember(UserEntity user, Update update, ChatMemberStatus status) : base(user, update)
        {
            _status = status;
        }
    }
}