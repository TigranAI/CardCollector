using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MyChatMember
{
    /* Родительский класс для входящих обновлений типа MyChatMember
     (Добавление/Добавление в чаты/Добавление в каналы/Блокировки/Исключения бота)
     Данный класс полностью реализован и не нуждается в наследовании */
    public class MyChatMember : UpdateModel
    {
        protected override string CommandText => "";
        private readonly ChatMemberStatus _status;
        public override Task Execute()
        {
            switch (_status)
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

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return true;
        }

        public static async Task<UpdateModel> Factory(Update update)
        {
            // Объект пользователя
            var user = await UserDao.GetUser(update.MyChatMember!.From);
            return new MyChatMember(user, update);
        }

        /*private static User ChatToUser(Chat chat)
        {
            return new User
            {
                Username = chat.Username,
                FirstName = chat.FirstName ?? chat.Title ?? "",
                LastName = chat.LastName ?? "",
                Id = chat.Id,
                IsBot = chat.Id < 0
            };
        }*/

        private MyChatMember(UserEntity user, Update update) : base(user, update)
        {
            _status = update.MyChatMember!.NewChatMember.Status;
        }
    }
}