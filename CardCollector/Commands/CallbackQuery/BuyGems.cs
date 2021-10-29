using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyGems : CallbackQueryCommand
    {
        protected override string CommandText => Command.buy_gems;
        
        public override async Task Execute()
        {
            await MessageController.SendInvoice(User, Text.gems_title, Text.gems_description, 
                Command.buy_gems_item, new[] {new LabeledPrice(Text.gems_label50, 100)},
                900000, new [] {400, 900, 2400, 4900}, Keyboard.BuyGemsKeyboard);
        }

        public BuyGems() { }
        public BuyGems(UserEntity user, Update update) : base(user, update) { }
    }
}