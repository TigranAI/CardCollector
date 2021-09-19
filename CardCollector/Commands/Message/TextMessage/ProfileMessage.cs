using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
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
            /* Открепляем все закрепы */
            await Bot.Client.UnpinAllChatMessages(User.ChatId);
            /* Удаляем предыдущее сообщение профиля пользователя */
            if (User.CurrentProfileMessageId != default) await MessageController.DeleteMessage(User, User.CurrentProfileMessageId);
            /* Отправляем сообщение */
            var message = await MessageController.SendMessage(User, 
                /* Имя пользователя */
                $"{User.Username}\n" +
                /* Количество монет */
                $"Монеты: {User.Cash.Coins}\n" +
                /* Количество алмазов */
                $"Алмазы: {User.Cash.Gems}",
                /* Клавиатура профиля */
                Keyboard.ProfileKeyboard);
            /* Записываем id нового сообщения */
            User.CurrentProfileMessageId = message.MessageId;
            /* Закрепляем новое сообщение профиля */
            await Bot.Client.PinChatMessageAsync(User.ChatId, message.MessageId, true);
        }
        
        public ProfileMessage() { }
        public ProfileMessage(UserEntity user, Update update) : base(user, update) { }
    }
}