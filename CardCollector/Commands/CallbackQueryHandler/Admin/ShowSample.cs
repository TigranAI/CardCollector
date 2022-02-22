using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    /* Этот класс можно использовать для тестирования или наброски эскизов
     Команда "Показать пример" доступна только пользователям с уровнем доступа "Разработчик" и выше
     PrivilegeLevel = 7 */
    public class ShowSample : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.show_sample;

        protected override async Task Execute()
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
        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public ShowSample(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}