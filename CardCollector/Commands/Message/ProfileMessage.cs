using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Команда "Профиль" Отображает профиль пользователя и его баланс */
    public class ProfileMessage : Message
    {
        /* Для данной команды ключевое слово "Профиль" */
        protected override string Command => MessageCommands.profile;
        public override async Task Execute()
        {
            /* Отправляем сообщение */
            await MessageController.SendMessage(User, 
                /* Имя пользователя */
                $"{User.Username}\n" +
                /* Количество монет */
                $"Монеты: {User.Cash.Coins}\n" +
                /* Количество алмазов */
                $"Алмазы: {User.Cash.Gems}",
                /* Клавиатура профиля */
                Keyboard.ProfileKeyboard);
        }
        
        public ProfileMessage() { }
        public ProfileMessage(UserEntity user, Update update) : base(user, update) { }
    }
}