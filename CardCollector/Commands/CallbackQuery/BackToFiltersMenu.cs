using System.Threading.Tasks;
using CardCollector.Commands.Message.TextMessage;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BackToFiltersMenu : CallbackQuery
    {
        protected override string CommandText => Command.back;
        public override async Task Execute()
        {
            /* Удаляем пользователя из очереди */
            EnterEmojiMessage.RemoveFromQueue(User.Id);
            /* Очищаем чат, если был передан параметр очистки */
            var clearChat = CallbackData.Contains(Command.clear_chat);
            if (clearChat) await User.ClearChat();
            /* Формируем сообщение с имеющимися фильтрами у пользователя */
            var text = User.Session.Filters.ToMessage(User.Session.State);
            /* Редактируем сообщение */
            if (!clearChat) 
                await MessageController.EditMessage(User, CallbackMessageId, 
                    text, Keyboard.GetSortingMenu(User.Session.State));
            else
            {
                var message = await MessageController.SendMessage(User, text, Keyboard.GetSortingMenu(User.Session.State));
                User.Session.Messages.Add(message.MessageId);
            }
        }
        
        public BackToFiltersMenu() { }
        public BackToFiltersMenu(UserEntity user, Update update) : base(user, update) { }
    }
}