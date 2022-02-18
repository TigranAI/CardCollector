using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Microsoft.EntityFrameworkCore;
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

        protected override async Task Execute()
        {
            var status = _chatMemberUpdated.NewChatMember.Status;
            switch (status)
            {
                case ChatMemberStatus.Member or ChatMemberStatus.Administrator:
                    await AddChat();
                    break;
                case ChatMemberStatus.Kicked or ChatMemberStatus.Left:
                    await BlockChat();
                    break;
            }
        }

        private async Task AddChat()
        {
            if (_chatMemberUpdated.Chat.Type is (ChatType.Group or ChatType.Supergroup or ChatType.Channel))
            {
                var telegramChat = await Context.TelegramChats
                    .SingleOrDefaultAsync(item => item.ChatId == _chatMemberUpdated.Chat.Id);
                if (telegramChat == null)
                {
                    telegramChat = (
                            await Context.TelegramChats.AddAsync(
                                new TelegramChat()
                                {
                                    ChatId = _chatMemberUpdated.Chat.Id,
                                    ChatType = _chatMemberUpdated.Chat.Type,
                                    Title = _chatMemberUpdated.Chat.Title
                                }
                            )
                    ).Entity;
                }
                telegramChat.IsBlocked = false;
            }
            else
            {
                User.IsBlocked = false;
            }
        }

        private async Task BlockChat()
        {
            if (_chatMemberUpdated.Chat.Type is (ChatType.Group or ChatType.Supergroup or ChatType.Channel))
            {
                var telegramChat = await Context.TelegramChats
                    .SingleOrDefaultAsync(item => item.ChatId == _chatMemberUpdated.Chat.Id);
                if (telegramChat == null) return;
                telegramChat.IsBlocked = true;
            }
            else
            {
                User.IsBlocked = true;
            }
        }

        public override bool Match() => true;

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.MyChatMember!.From);

            user.InitSession();

            return new MyChatMemberCommand(user, context, update.MyChatMember);
        }

        private MyChatMemberCommand(User user, BotDatabaseContext context, ChatMemberUpdated member) : base(user,
            context)
        {
            _chatMemberUpdated = member;
        }
    }
}