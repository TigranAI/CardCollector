using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DailyTasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class DailyTasks : CallbackQueryCommand
    {
        protected override string CommandText => Command.daily_tasks;
        
        public override async Task Execute()
        {
            await User.ClearChat();
            var text = Messages.your_daily_tasks;
            var userTasks = await DailyTaskDao.GetUserTasks(User.Id);
            foreach (var (key, task) in DailyTask.List)
            {
                if (!userTasks.ContainsKey((int)key)) 
                    userTasks.Add((int)key, await DailyTaskDao.AddNew(User.Id, (int)key));
                text += $"\n{task.Title} ({task.Goal - userTasks[(int) key].Progress}/{task.Goal})";
            }
            var message = await MessageController.SendMessage(User, text);
            User.Session.Messages.Add(message.MessageId);
        }

        public DailyTasks() { }
        public DailyTasks(UserEntity user, Update update) : base(user, update) { }
    }
}