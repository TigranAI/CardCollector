using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.Message
{
    public class ProfileMessage : Message
    {
        protected override string Command => "Профиль";
        public override async Task Execute()
        {
            var keyboard = new InlineKeyboardMarkup(new[]
                {
                    InlineKeyboardButton.WithCallbackData("Собрать прибыль")
                }
            );
            await MessageController.SendMessage(
                User, 
                $"{User.Username}\n" +
                       $"Монеты: {User.Cash.Coins}\n" +
                       $"Алмазы: {User.Cash.Gems}",
                keyboard);
        }
        
        public ProfileMessage() { }
        public ProfileMessage(UserEntity user, Update update) : base(user, update) { }
    }
}