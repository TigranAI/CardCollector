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
    public class EnterGemsPrice : MessageCommand
    {
        protected override string CommandText => "";
        
        private static readonly List<long> Queue = new ();
        public override async Task Execute()
        {
            /* если пользователь ввел что-то кроме эмодзи */
            var module = User.Session.GetModule<CollectionModule>();
            if (!int.TryParse(Update.Message!.Text, out var price) || price < 0)
            {
                await MessageController.EditMessage(User,
                    $"{Messages.current_price} {module.SellPrice}{Text.gem}\n{Messages.please_enter_integer}",
                    Keyboard.AuctionPutCancelKeyboard);
            }
            else
            {
                module.SellPrice = price;
                await MessageController.EditMessage(User,
                    $"{Messages.confirm_selling} {module.SellPrice}{Text.gem}:", Keyboard.AuctionPutCancelKeyboard);
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

        public EnterGemsPrice() { }
        public EnterGemsPrice(UserEntity user, Update update) : base(user, update) { }
    }
}
