using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

namespace CardCollector.Commands.CallbackQuery
{
    public class SetGemsSum : CallbackQueryHandler
    {
        protected override string CommandText => Command.set_gems_sum;
        
        public override async Task Execute()
        {
            var count = int.Parse(CallbackData.Split('=')[1]);
            var label = string.Format(Text.gems_title, count, count / 100 * 69);
            var description = string.Format(Text.gems_description, count);
            await MessageController.SendInvoice(User, label, description, Command.buy_gems_item, 
                new[] {new LabeledPrice(label, count*69)}, Keyboard.BuyGemsKeyboard(count), Currency.RUB);
        }

        public SetGemsSum(UserEntity user, Update update) : base(user, update) { }
    }
}