using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    /* Команда "Профиль" Отображает профиль пользователя и его баланс */
    public class ProfileMessage : Message
    {
        /* Для данной команды ключевое слово "Профиль" */
        protected override string CommandText => Text.profile;
        public override async Task Execute()
        {
            /* Подсчитываем прибыль */
            await User.Session.CalculateIncome();
            /* Отправляем сообщение */
            var message = await MessageController.SendMessage(User, 
                /* Имя пользователя */
                $"{User.Username}\n" +
                /* Количество монет */
                $"{Messages.coins}: {User.Cash.Coins}{Text.coin}\n" +
                /* Количество алмазов */
                $"{Messages.gems}: {User.Cash.Gems}{Text.gem}",
                /* Клавиатура профиля */
                Keyboard.GetProfileKeyboard(User));
            /* Записываем id нового сообщения */
            User.Session.Messages.Add(message.MessageId);
        }
        
        public ProfileMessage() { }
        public ProfileMessage(UserEntity user, Update update) : base(user, update) { }
    }
}