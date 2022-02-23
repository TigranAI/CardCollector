using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Logs;
using CardCollector.Commands.ChosenInlineResultHandler.Group;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.UserDailyTask;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler.Private
{
    [SavedActivity]
    public class SendPrivateSticker : ChatSendSticker
    {
        protected override string CommandText => ChosenInlineResultCommands.send_private_sticker;

        protected override async Task Execute()
        {
            var dailyTask = GetTaskInfo();
            if (dailyTask.Progress == 0) return;
            dailyTask.Progress--;
            await SendAlert(dailyTask);
            await GivePrize(dailyTask);
        }

        private DailyTask GetTaskInfo()
        {
            var dailyTask = User.DailyTasks.SingleOrDefault(item => item.TaskId == TaskKeys.SendStickersToUsers);
            if (dailyTask is null)
            {
                dailyTask = new DailyTask()
                {
                    User = User,
                    TaskId = TaskKeys.SendStickersToUsers,
                    Progress = TaskGoals.Goals[TaskKeys.SendStickersToUsers]
                };
                User.DailyTasks.Add(dailyTask);
            }

            return dailyTask;
        }

        private async Task GivePrize(DailyTask dailyTask)
        {
            if (dailyTask.Progress == 0)
            {
                var packInfo = await Context.Packs.FindById(1);
                User.AddPack(packInfo, 1);
                await User.Messages.SendDailyTaskComplete(User);
            }
        }

        private async Task SendAlert(DailyTask dailyTask)
        {
            if (User.Settings[Resources.Enums.UserSettings.DailyTaskProgress])
                await User.Messages.SendDailyTaskProgress(User,
                    $"{Messages.send_sticker_progress}: " +
                    $"{TaskGoals.Goals[TaskKeys.SendStickersToUsers] - dailyTask.Progress}" +
                    $" / {TaskGoals.Goals[TaskKeys.SendStickersToUsers]}");
        }

        public SendPrivateSticker(User user, BotDatabaseContext context, ChosenInlineResult chosenInlineResult) : base(user, context, chosenInlineResult)
        {
        }
    }
}