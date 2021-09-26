using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    public class EnterPriceMessage : Message
    {
        protected override string CommandText => "";
        private const string pattern = @"^[0-9]+$";

        //Список пользователей, от которых ожидается ввод эмоджи ключ - id пользователя, значение - сообщение с меню #1#
        private static readonly Dictionary<long, int> Queue = new ();

        public override async Task Execute()
        {
            
            var input = "" + Update.Message!
                .Text; // это чтоб варн не показывлся, тут никогда не null, так как в Factory все уже проверено
            /* если пользователь ввел что-то кроме эмодзи */
            if (!Regex.IsMatch(input, pattern))
                await MessageController.EditMessage(User, Queue[User.Id], Messages.please_enter_price,
                    Keyboard.CancelKeyboard);
            else
            {
                User.Session.CoinPrice = Convert.ToInt32(input);
                /* Редактируем сообщение */
                await MessageController.EditMessage(User, Queue[User.Id], "Введите алмазы", Keyboard.CancelKeyboard);
                EnterPricePart2Message.AddToQueue(User.Id,Queue[User.Id]);
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

        public EnterPriceMessage() { }
        public EnterPriceMessage(UserEntity user, Update update) : base(user, update) { }
        }
}
