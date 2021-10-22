using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message.TextMessage
{
    public class EnterGemsExchange : MessageCommand
    {
        protected override string CommandText => "";
        
        private static readonly Dictionary<long, int> Queue = new ();
        public override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            if (!int.TryParse(Update.Message!.Text, out var sum) || sum < 0)
                await MessageController.EditMessage(User, Queue[User.Id], 
                    $"{Messages.exchange_mesage}" +
                    $"\n{Messages.gems_exchange_count} {module.EnteredExchangeSum}{Text.gem}" +
                    $"\n{Messages.coins_exchange_count} {module.EnteredExchangeSum * 150}{Text.coin}" +
                    $"\n{Messages.please_enter_integer}",
                    Keyboard.BuyCoinsKeyboard);
            else
            {
                module.EnteredExchangeSum = sum;
                await MessageController.EditMessage(User, Queue[User.Id], 
                    $"{Messages.exchange_mesage}" +
                    $"\n{Messages.gems_exchange_count} {module.EnteredExchangeSum}{Text.gem}" +
                    $"\n{Messages.coins_exchange_count} {module.EnteredExchangeSum * 150}{Text.coin}" +
                    $"\n{Messages.confirm_exchange}",
                    Keyboard.BuyCoinsKeyboard);
                Queue.Remove(User.Id);
            }
        }

        //Добавляем пользователя в очередь #1#
        public static void AddToQueue(long userId, int messageId)
        {
            Queue.TryAdd(userId, messageId);
        }

        //Удаляем пользователя из очереди #1#
        public static void RemoveFromQueue(long userId)
        {
            Queue.Remove(userId);
        }

        // Переопределяем метод, так как команда удовлетворяет условию, если пользователь находится в очереди #1#
        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return Queue.ContainsKey(user.Id) && update.Message!.Type == MessageType.Text;
        }

        public EnterGemsExchange() { }

        public EnterGemsExchange(UserEntity user, Update update) : base(user, update) { }
    }
}