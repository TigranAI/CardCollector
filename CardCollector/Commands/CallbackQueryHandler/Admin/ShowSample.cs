using System.Threading.Tasks;
using CardCollector.Resources.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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
            
        }

        /* Нужно помимо совпадения текста проверить пользователя на уровень привилегий */
        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }
    }
}