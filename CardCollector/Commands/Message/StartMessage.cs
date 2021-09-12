using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.Message
{
    public class StartMessage:Message
    {
        public StartMessage(UserEntity user, Update update) : base(user, update)
        {
            
        }
        public StartMessage():base(null,null)
        {
            
        }

        protected override string Command => "/start";
        
        public override async Task<Telegram.Bot.Types.Message> Execute()
        {
            var keyboard = new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton("Профиль"),
                new KeyboardButton("Коллекция"),
                new KeyboardButton("Магазин"),
                new KeyboardButton("Аукцион")
            });
            keyboard.ResizeKeyboard = true;
            return await MessageController.SendMessage(User,$"Привет!", keyboard);
        }
    }
}