using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    public class EnterGemsExchange : MessageCommand
    {
        protected override string CommandText => "";

        private static readonly List<long> Queue = new ();
        public override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            if (!int.TryParse(Update.Message!.Text, out var sum) || sum < 0)
                await MessageController.EditMessage(User,
                    $"{Messages.exchange_mesage}" +
                    $"\n{Messages.gems_exchange_count} {module.EnteredExchangeSum}{Text.gem}" +
                    $"\n{Messages.coins_exchange_count} {module.EnteredExchangeSum * 150}{Text.coin}" +
                    $"\n\n{Messages.please_enter_integer}",
                    Keyboard.BuyCoinsKeyboard);
            else
            {
                module.EnteredExchangeSum = sum; 
                await MessageController.EditMessage(User,
                    $"{Messages.exchange_mesage}" +
                    $"\n{Messages.gems_exchange_count} {module.EnteredExchangeSum}{Text.gem}" +
                    $"\n{Messages.coins_exchange_count} {module.EnteredExchangeSum * 150}{Text.coin}" +
                    $"\n\n{Messages.confirm_exchange}",
                    Keyboard.BuyCoinsKeyboard);
                Queue.Remove(User.Id);
            }
        }

        /* Добавляем пользователя в очередь */
        public static void AddToQueue(long userId)
        {
            if (!Queue.Contains(userId)) Queue.Add(userId);
        }

        /* Удаляем пользователя из очереди */
        public static void RemoveFromQueue(long userId)
        {
            Queue.Remove(userId);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return Queue.Contains(user.Id) && update.Message!.Type == MessageType.Text;
        }

        public EnterGemsExchange() { }

        public EnterGemsExchange(UserEntity user, Update update) : base(user, update) { }
    }
}