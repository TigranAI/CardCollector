using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.UserDailyTask;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

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
                var title = Titles.ResourceManager.GetString(task.TaskId.ToString());
                var goal = TaskGoals.Goals[task.TaskId];
                text += $"\n{title} ({goal - task.Progress}/{goal})";
            }
            await User.Messages.EditMessage(User, text, Keyboard.BackKeyboard);
        }

        public DailyTasks(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}