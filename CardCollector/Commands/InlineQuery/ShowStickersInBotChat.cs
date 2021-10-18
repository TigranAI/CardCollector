using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQuery
{
    /* Отображение стикеров в личной беседt с ботом */
    public class ShowStickersInBotChat : InlineQuery
    {
        /* Команда - пустая строка, поскольку пользователь может вводить любые слова
         после @имя_бота, введенная фраза будет использоваться для фильтрации стикеров */
        protected override string CommandText => "";
        
        public override async Task Execute()
        {
            // Получаем список стикеров
            var stickersList = await User.GetStickersList(Query);
            var results = stickersList.ToTelegramResults(Command.select_sticker);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return update.InlineQuery?.ChatType is ChatType.Sender && user.Session.State == UserState.Default;
        }

        public ShowStickersInBotChat() { }
        public ShowStickersInBotChat(UserEntity user, Update update) : base(user, update) { }
    }
}