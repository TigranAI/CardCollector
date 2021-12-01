using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQuery
{
    /* Отображение стикеров в чатах, кроме личной беседы с ботом */
    public class ShowStickersInGroup : InlineQueryCommand
    {
        public override async Task Execute()
        {
            // Получаем список стикеров
            var stickersList = await User.GetStickersList(Query);
            var results = stickersList.ToTelegramResults(Command.give_exp, false);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
        }
        
        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return update.InlineQuery?.ChatType is ChatType.Group or ChatType.Supergroup;
        }

        public ShowStickersInGroup() { }
        public ShowStickersInGroup(UserEntity user, Update update) : base(user, update) { }
    }
}