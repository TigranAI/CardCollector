using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyPackQuery : CallbackQuery
    {
        protected override string CommandText => Command.buy_pack;
        public override async Task Execute()
        {
            await MessageController.EditMessage(User, CallbackMessageId, Messages.choose_option,
                Keyboard.ShopPacksKeyboard);
        }

        public BuyPackQuery() { }
        public BuyPackQuery(UserEntity user, Update update) : base(user, update) { }
    }
}