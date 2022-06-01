using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Resources.Translations.Providers;
using CardCollector.UserDailyTask;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    public class DailyTasks : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.daily_tasks;

        protected override async Task Execute()
        {
            var text = Messages.your_daily_tasks;
            foreach (var task in User.DailyTasks)
            {
                var title = TitlesProvider.Instance[task];
                var goal = TaskGoals.Goals[task.TaskId];
                text += $"\n{title} ({goal - task.Progress}/{goal})";
            }
            await User.Messages.EditMessage(text, Keyboard.BackKeyboard);
        }
    }
}