using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.Message
{
    public class StartMessage : Message
    {
        protected override string Command => "/start";
        
        public override async Task Execute()
        {
            var keyboard = new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton[] {"Профиль", "Коллекция"},
                new KeyboardButton[] {"Магазин", "Аукцион"},
            }) { ResizeKeyboard = true };
            await MessageController.SendMessage(User,"Привет!", keyboard);
        }
        
        public StartMessage(UserEntity user, Update update) : base(user, update) { }
        public StartMessage() { }
    }
}