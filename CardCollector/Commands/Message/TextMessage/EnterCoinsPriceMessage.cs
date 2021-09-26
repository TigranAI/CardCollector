using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    public class EnterCoinsPriceMessage : Message
    {
        protected override string CommandText => "";

        //Список пользователей, от которых ожидается ввод эмоджи ключ - id пользователя, значение - сообщение с меню #1#
        private static readonly Dictionary<long, int> Queue = new ();

        public override async Task Execute()
        {
            if (!int.TryParse(Update.Message!.Text, out var price) || price < 0)
                await MessageController.EditMessage(User, Queue[User.Id], 
                    $"{Messages.current_price} {User.Session.CoinPrice}{Text.coin} / {User.Session.GemPrice}{Text.gem}" +
                    $"\n{Messages.please_enter_price}", Keyboard.AuctionPutCancelKeyboard);
            else
            {
                User.Session.CoinPrice = price;
                /* Редактируем сообщение */
                await MessageController.EditMessage(User, Queue[User.Id], 
                    $"{Messages.current_price} {User.Session.CoinPrice}{Text.coin} / {User.Session.GemPrice}{Text.gem}" +
                    $"{Messages.enter_your_gems_price} {Text.gem}:", Keyboard.AuctionPutCancelKeyboard);
                EnterGemsPriceMessage.AddToQueue(User.Id,Queue[User.Id]);
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
        protected internal override bool IsMatches(string command)
        {
            return User == null ? base.IsMatches(command) : Queue.ContainsKey(User.Id);
        }

        public EnterCoinsPriceMessage() { }
        public EnterCoinsPriceMessage(UserEntity user, Update update) : base(user, update) { }
    }
}
