using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Shop
{
    public class EnterGemsExchange : MessageHandler
    {
        protected override string CommandText => "";

        private static readonly LinkedList<long> Queue = new ();
        protected override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            if (!int.TryParse(Message.Text, out var sum) || sum < 0)
                await User.Messages.EditMessage(User,
                    $"{Messages.exchange_mesage}" +
                    $"\n{Messages.gems_exchange_count} {module.EnteredExchangeSum}{Text.gem}" +
                    $"\n{Messages.coins_exchange_count} {module.EnteredExchangeSum * 10}{Text.coin}" +
                    $"\n\n{Messages.please_enter_integer}",
                    Keyboard.BuyCoinsKeyboard());
            else
            {
                module.EnteredExchangeSum = sum; 
                await User.Messages.EditMessage(User,
                    $"{Messages.exchange_mesage}" +
                    $"\n{Messages.gems_exchange_count} {module.EnteredExchangeSum}{Text.gem}" +
                    $"\n{Messages.coins_exchange_count} {module.EnteredExchangeSum * 10}{Text.coin}" +
                    $"\n\n{Messages.confirm_exchange}",
                    Keyboard.BuyCoinsKeyboard());
                Queue.Remove(User.Id);
            }
        }

        /* Добавляем пользователя в очередь */
        public static void AddToQueue(long userId)
        {
            if (!Queue.Contains(userId)) Queue.AddLast(userId);
        }

        /* Удаляем пользователя из очереди */
        public static void RemoveFromQueue(long userId)
        {
            Queue.Remove(userId);
        }

        public override bool Match()
        {
            return Queue.Contains(User.Id) && Message.Type == MessageType.Text;
        }
    }
}