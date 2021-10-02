using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.Message.TextMessage
{
    /* Этот класс можно использовать для тестирования или наброски эскизов
     Команда "Показать пример" доступна только пользователям с уровнем доступа "Разработчик" и выше
     PrivilegeLevel = 5 */
    public class ShowSampleMessage : Message
    {
        protected override string CommandText => Text.show_sample;
        public override async Task Execute()
        {
            await Bot.Client.SendInvoiceAsync(User.ChatId, "test", "test", "test", "401643678:TEST:f13667cd-bbf7-4ca1-ba9e-7aa49e4d3faa",
                "USD", new []
                {
                    new LabeledPrice("text", 100)
                });
        }

        /* Нужно помимо совпадения текста проверить пользователя на уровень привилегий */
        protected internal override bool IsMatches(string command)
        {
            return base.IsMatches(command) && User is not {PrivilegeLevel: < Constants.PROGRAMMER_PRIVILEGE_LEVEL};
        }
        
        public ShowSampleMessage(UserEntity user, Update update) : base(user, update) { }
        public ShowSampleMessage() { }
    }
}