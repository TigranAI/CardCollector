using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowStickersInPrivate : InlineQueryHandler
    {
        public override async Task Execute()
        {
            // Получаем список стикеров
            var stickersList = await User.GetStickersList(Query);
            var results = stickersList.ToTelegramResults(Command.send_private_sticker, asMessage:false);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
        }
        
        protected internal override bool Match(UserEntity user, Update update)
        {
            return update.InlineQuery?.ChatType is ChatType.Private;
        }
        
        public ShowStickersInPrivate(UserEntity user, Update update) : base(user, update) { }
    }
}