using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Group
{
    public class ShowRules : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.send_rules;
        protected override async Task Execute()
        {
            var game = (GamesType) int.Parse(CallbackQuery.Data!.Split("=")[1]);
            await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.rules_sent_to_private);
            await User.Messages.SendMessage(GetRules(game), Keyboard.BackKeyboard);
        }

        private string GetRules(GamesType type)
        {
            return type switch
            {
                GamesType.Roulette => Messages.roulette_rules,
                GamesType.Puzzle => Messages.puzzle_rules,
                _ => ""
            };
        }
    }
}