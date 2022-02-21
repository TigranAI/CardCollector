using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

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
        
        public RouletteRules(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}