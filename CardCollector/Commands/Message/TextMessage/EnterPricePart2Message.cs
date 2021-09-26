using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    public class EnterPricePart2Message : Message
    {
        private const string pattern = @"^[0-9]+$";
        protected override string CommandText => "";
        
        private static readonly Dictionary<long, int> Queue = new ();
        public override async Task Execute()
        {
            var input = "" + Update.Message!.Text;
            /* если пользователь ввел что-то кроме эмодзи */
            if (!Regex.IsMatch(input, pattern))
                await MessageController.EditMessage(User, Queue[User.Id], Messages.please_enter_price,
                    Keyboard.CancelKeyboard);
            else
            {
                User.Session.GemPrice = Convert.ToInt32(input);
                await MessageController.EditMessage(User, Queue[User.Id],
                    "Подтвердите сумму" + $"{User.Session.CoinPrice}" +
                    " Монет и" + $"{User.Session.GemPrice}" + "Алмазов",Keyboard.GetConfirmationKeyboard(Command.command_yes_on_auction));
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

        public EnterPricePart2Message()
        {
        }

        public EnterPricePart2Message(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}
