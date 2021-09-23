using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowAuctionStickers : InlineQuery
    {
        protected override string CommandText => "";
        public override async Task Execute()
        {
            // Фильтр - введенная пользователем фраза
            var filter = Update.InlineQuery!.Query;
            // Получаем список стикеров
            var stickersList = await AuctionController.GetStickers(filter);
            var results = User.Session.Filters.ApplyTo(stickersList, User.Session.State).ToTelegramResults(Command.select_sticker);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
        }
        
        /* Команда пользователя удовлетворяет условию, если она вызвана
         в личных сообщениях с ботом и пользователь в меню аукциона и он не выбрал стикер */
        protected internal override bool IsMatches(string command)
        {
            return User == null 
                ? command.Contains("Sender")
                : User.Session.State == UserState.AuctionMenu && User.Session.SelectedSticker == null;
        }

        public ShowAuctionStickers() { }
        public ShowAuctionStickers(UserEntity user, Update update, string inlineQueryId) : base(user, update, inlineQueryId) { }
    }
}