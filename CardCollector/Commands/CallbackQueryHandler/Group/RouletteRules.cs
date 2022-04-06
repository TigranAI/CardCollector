using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Group
{
    public class RouletteRules : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.roulette_rule;
        protected override async Task Execute()
        {
            await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.rules_sent_to_private);
            await User.Messages.SendMessage(User, Messages.roulette_rules, Keyboard.BackKeyboard);
        }
    }
}