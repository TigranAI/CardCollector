using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class PriceCallback : CallbackQuery
    {
        protected override string CommandText => Command.price;
        public override async Task Execute()
        {
            await MessageController.EditMessage(User, CallbackMessageId, Messages.choose_price, Keyboard.PriceOptions);
        }

        public PriceCallback() { }
        public PriceCallback(UserEntity user, Update update) : base(user, update) { }
    }
}