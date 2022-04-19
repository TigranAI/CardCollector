using System.Threading.Tasks;
using CardCollector.Commands.PreCheckoutQueryHandler;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Payments;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    public class SetGemsSum : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.set_gems_sum;
        
        protected override async Task Execute()
        {
            var count = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            var label = string.Format(Text.gems_title, count, count / 5);
            var description = string.Format(Text.gems_description, count);
            await MessageController.SendInvoice(User, label, description, PreCheckoutQueryCommands.buy_gems_item, 
                new[] {new LabeledPrice(label, count * 100 / 5)}, Keyboard.BuyGemsKeyboard(count));
        }
    }
}