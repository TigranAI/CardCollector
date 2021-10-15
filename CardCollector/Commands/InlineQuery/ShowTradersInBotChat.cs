using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
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
            var module = User.Session.GetModule<AuctionModule>();
            // Получаем список продавцов
            var traders = await AuctionController.GetTradersList(filter, module.SelectedSticker.Id);
            var results = User.Session.GetModule<FiltersModule>()
                .ApplyPriceTo(traders)
                .ToTelegramResults(Command.buy_sticker, 1.0 - await User.AuctionDiscount()/100.0);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, await results);
        }
        
        /* Команда пользователя удовлетворяет условию, если она вызвана
         в личных сообщениях с ботом и пользователь выбрал стикер в меню аукциона */
        protected internal override bool IsMatches(string command)
        {
            return User == null 
                ? command.Contains("Sender")
                : User.Session.State == UserState.ProductMenu;
        }

        public ShowTradersInBotChat() { }
        public ShowTradersInBotChat(UserEntity user, Update update) : base(user, update) { }
    }
}