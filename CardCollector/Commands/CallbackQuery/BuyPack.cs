using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyPack : CallbackQueryCommand
    {
        protected override string CommandText => Command.buy_pack;
        protected override bool AddToStack => true;
        
        public override async Task Execute()
        {
            await MessageController.SendMessage(User, Messages.choose_option, Keyboard.ShopPacksKeyboard);
        }

        public BuyPack() { }
        public BuyPack(UserEntity user, Update update) : base(user, update) { }
    }
}