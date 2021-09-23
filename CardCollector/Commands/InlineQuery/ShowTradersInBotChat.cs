using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowTradersInBotChat : InlineQuery
    {
        protected override string CommandText => "";
        public override async Task Execute()
        {
            // Фильтр - введенная пользователем фраза
            var filter = Update.InlineQuery!.Query;
            // Получаем список продавцов
            var traders = await AuctionController.GetTradersList(filter, User.Session.SelectedSticker.Id);
            var results = traders.ToTelegramResults(Command.buy_sticker);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
        }
        
        /* Команда пользователя удовлетворяет условию, если она вызвана
         в личных сообщениях с ботом и пользователь выбрал стикер в меню аукциона */
        protected internal override bool IsMatches(string command)
        {
            return User == null 
                ? command.Contains("Sender")
                : User.Session.SelectedSticker != null && User.Session.State == UserState.AuctionMenu;
        }

        public ShowTradersInBotChat() { }
        public ShowTradersInBotChat(UserEntity user, Update update, string inlineQueryId) : base(user, update, inlineQueryId) { }
    }
}