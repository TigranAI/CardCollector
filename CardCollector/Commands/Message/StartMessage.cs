using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.Message
{
    public class StartMessage : Message
    {
        protected override string Command => "/start";
        
        public override async Task<Telegram.Bot.Types.Message> Execute()
        {
            var keyboard = new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton[] {"Профиль", "Коллекция"},
                new KeyboardButton[] {"Магазин", "Аукцион"},
            }) { ResizeKeyboard = true };
            return await MessageController.SendMessage(User,"Привет!", keyboard);
        }
        
        public StartMessage(UserEntity user, Update update) : base(user, update) { }
        public StartMessage() { }
    }
}