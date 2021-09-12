using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.Message
{
    public class ProfileMessage:Message
    {
        public ProfileMessage():base(null,null)
        {
            
        }
        public ProfileMessage(UserEntity user, Update update) : base(user, update)
        {
            
        }

        protected override string Command => "Профиль";
        public override async Task<Telegram.Bot.Types.Message> Execute()
        {
            var keyboard = new InlineKeyboardMarkup(new[]
                {
                    InlineKeyboardButton.WithCallbackData("Собрать прибыль")
                }
            );
            return await MessageController.SendMessage(User,$"{User.Username}", keyboard);
        }
    }
}