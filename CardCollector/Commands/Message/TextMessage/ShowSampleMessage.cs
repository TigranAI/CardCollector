using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

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
            await Bot.Client.SendInvoiceAsync(User.ChatId, "test", "test", "test", AppSettings.PAYMENT_PROVIDER,
                "USD", new []
                {
                    new LabeledPrice("text", 100)
                });
        }

        /* Нужно помимо совпадения текста проверить пользователя на уровень привилегий */
        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && User.PrivilegeLevel >= Constants.PROGRAMMER_PRIVILEGE_LEVEL;
        }
        
        public ShowSampleMessage(UserEntity user, Update update) : base(user, update) { }
        public ShowSampleMessage() { }
    }
}