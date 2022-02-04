using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQuery
{
    /* Отображение стикеров в чатах, кроме личной беседы с ботом */
    public class ShowStickersInGroup : InlineQueryHandler
    {
        public override async Task Execute()
        {
            // Получаем список стикеров
            var stickersList = await User.GetStickersList(Query);
            var offset = int.Parse(Update.InlineQuery.Offset == "" ? "0" : Update.InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count() ? "" : (offset + 50).ToString();
            var results = stickersList.ToTelegramResults(Command.give_exp, offset, false);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results, newOffset);
        }
        
        protected internal override bool Match(UserEntity user, Update update)
        {
            return update.InlineQuery?.ChatType is ChatType.Group or ChatType.Supergroup or ChatType.Channel;
        }

        public ShowStickersInGroup(UserEntity user, Update update) : base(user, update) { }
    }
}