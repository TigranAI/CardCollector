using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.CallbackQuery
{
    /* Этот класс можно использовать для тестирования или наброски эскизов
     Команда "Показать пример" доступна только пользователям с уровнем доступа "Разработчик" и выше
     PrivilegeLevel = 7 */
    public class ShowSample : CallbackQueryCommand
    {
        protected override string CommandText => Command.show_sample;

        public override async Task Execute()
        {
            var loginUrl = new LoginUrl
            {
                Url = "http://127.0.0.1:8081/login"
            };
            await Bot.Client.SendTextMessageAsync(User.ChatId, "Test", replyMarkup: new InlineKeyboardMarkup(
                new []
                {
                    InlineKeyboardButton.WithLoginUrl("test", loginUrl), 
                }));
        }

        /* Нужно помимо совпадения текста проверить пользователя на уровень привилегий */
        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public ShowSample() { }
        public ShowSample(UserEntity user, Update update) : base(user, update) { }
    }
}