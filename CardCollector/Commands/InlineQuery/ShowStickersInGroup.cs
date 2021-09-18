using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.InlineQuery
{
    /* Отображение стикеров в чатах, кроме личной беседы с ботом */
    public class ShowStickersInGroup : InlineQuery
    {
        /* Команда - пустая строка, поскольку пользователь может вводить любые слова
         после @имя_бота, введенная фраза будет использоваться для фильтрации стикеров */
        protected override string CommandText => "";
        
        public override async Task Execute()
        {
            // Фильтр - введенная пользователем фраза
            var filter = Update.InlineQuery!.Query;
            // Получаем список стикеров
            var stickersList = await User.GetStickersList(Command.send_sticker, filter);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, stickersList);
        }

        /* Команда пользователя удовлетворяет условию, если она вызвана
         в беседе/канале/личных сообщениях (кроме личных сообщений с ботом) */
        protected internal override bool IsMatches(string command)
        {
            return command.Contains("Group") || command.Contains("Supergroup") || command.Contains("Private");
        }

        public ShowStickersInGroup(UserEntity user, Update update, string inlineQueryId)
            : base(user, update, inlineQueryId) { }
        public ShowStickersInGroup() { }
    }
}