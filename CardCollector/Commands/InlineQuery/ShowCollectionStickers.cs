using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowCollectionStickers : InlineQuery
    {
        protected override string CommandText => "";
        public override async Task Execute()
        {
            // Фильтр - введенная пользователем фраза
            var filter = Update.InlineQuery!.Query;
            // Получаем список стикеров
            var stickersList = await User.GetStickersList(filter);
            var results = User.Session.Filters
                .ApplyTo(stickersList, User.Session.State).ToTelegramResults(Command.select_sticker);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
        }
        
        /* Команда пользователя удовлетворяет условию, если она вызвана
         в личных сообщениях с ботом и пользователь в меню коллекции */
        protected internal override bool IsMatches(string command)
        {
            return User == null 
                ? command.Contains("Sender")
                : User.Session.State == UserState.CollectionMenu;
        }

        public ShowCollectionStickers() { }
        public ShowCollectionStickers(UserEntity user, Update update, string inlineQueryId) : base(user, update, inlineQueryId) { }
    }
}