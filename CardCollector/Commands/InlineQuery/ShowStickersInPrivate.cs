using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowStickersInPrivate : InlineQuery
    {
        /* Команда - пустая строка, поскольку пользователь может вводить любые слова
         после @имя_бота, введенная фраза будет использоваться для фильтрации стикеров */
        protected override string CommandText => "";
        
        public override async Task Execute()
        {
            // Фильтр - введенная пользователем фраза
            var filter = Update.InlineQuery!.Query;
            // Получаем список стикеров
            var stickersList = await User.GetStickersList(filter);
            var results = stickersList.ToTelegramResults(Command.send_private_sticker, false);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
        }

        /* Команда пользователя удовлетворяет условию, если она вызвана
         в личных сообщениях (кроме личных сообщений с ботом) */
        protected internal override bool IsMatches(string command)
        {
            return command.Contains("Private");
        }

        public ShowStickersInPrivate() { }
        public ShowStickersInPrivate(UserEntity user, Update update) : base(user, update) { }
    }
}