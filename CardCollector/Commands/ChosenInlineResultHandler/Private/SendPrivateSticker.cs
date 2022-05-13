using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.ChosenInlineResultHandler.Group;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.UserDailyTask;

namespace CardCollector.Commands.ChosenInlineResultHandler.Private
{
    [Statistics]
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

            if (User.InviteInfo?.TasksProgress is { } tp
                && tp.SendStickersToPrivate < BeginnersTasksProgress.SendStickersGoalToPrivate)
            {
                tp.SendStickersToPrivate++;
                await User.InviteInfo.CheckRewards(Context);
            }

            var userStickerId = long.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
            User.Stickers.SingleOrDefault(item => item.Id == userStickerId)?.UpdateLastUsage();
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
                await User.Stickers
                    .Where(sticker => sticker.Sticker.ExclusiveTask is ExclusiveTask.CompleteDailyTask)
                    .Apply(async sticker => await sticker.DoExclusiveTask());
            }
        }

        private async Task SendAlert(DailyTask dailyTask)
        {
            if (User.Settings[UserSettingsTypes.DailyTaskProgress])
                await User.Messages.SendDailyTaskProgress(User,
                    $"{Messages.send_sticker_progress}: " +
                    $"{TaskGoals.Goals[TaskKeys.SendStickersToUsers] - dailyTask.Progress}" +
                    $" / {TaskGoals.Goals[TaskKeys.SendStickersToUsers]}");
        }
    }
}