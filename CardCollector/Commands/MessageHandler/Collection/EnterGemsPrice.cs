using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Collection
{
    public class EnterGemsPrice : MessageHandler
    {
        protected override string CommandText => "";
        
        private static readonly List<long> Queue = new ();
        
        protected override async Task Execute()
        {
            var module = User.Session.GetModule<CollectionModule>();
            if (!int.TryParse(Message.Text, out var price) || price < 0)
            {
                await User.Messages.EditMessage(User,
                    $"{Messages.current_price} {module.SellPrice}{Text.gem}\n{Messages.please_enter_integer}",
                    Keyboard.AuctionPutCancelKeyboard);
            }
            else
            {
                module.SellPrice = price;
                await User.Messages.EditMessage(User, $"{Messages.confirm_selling} {module.SellPrice}{Text.gem}:" +
                                                      $"\n{Messages.or_enter_another_sum}", Keyboard.AuctionPutCancelKeyboard);
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

        public override bool Match()
        {
            return Queue.Contains(User.Id) && Message.Type == MessageType.Text;
        }

        public EnterGemsPrice(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}
