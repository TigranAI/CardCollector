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
    public class EnterGemsPriceMessage : Message
    {
        protected override string CommandText => "";
        
        private static readonly Dictionary<long, int> Queue = new ();
        public override async Task Execute()
        {
            /* если пользователь ввел что-то кроме эмодзи */
            var module = User.Session.GetModule<CollectionModule>();
            if (!int.TryParse(Update.Message!.Text, out var price) || price < 0)
                await MessageController.EditMessage(User, Queue[User.Id], 
                    $"{Messages.current_price} {module.SellPrice}{Text.gem}\n{Messages.please_enter_price}",
                    Keyboard.AuctionPutCancelKeyboard);
            else
            {
                module.SellPrice = price;
                await MessageController.EditMessage(User, Queue[User.Id],
                    $"{Messages.confirm_selling} {module.SellPrice}{Text.gem}:", Keyboard.AuctionPutCancelKeyboard);
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
            return Queue.ContainsKey(User.Id) && update.Message!.Type == MessageType.Text;
        }

        public EnterGemsPriceMessage() { }

        public EnterGemsPriceMessage(UserEntity user, Update update) : base(user, update) { }
    }
}
