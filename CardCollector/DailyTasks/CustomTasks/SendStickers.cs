using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.DailyTasks.CustomTasks
{
    public class SendStickers : DailyTask
    {
        public override int Id => (int)DailyTaskKeys.SendStickersToUsers;
        public override int Goal => 5;
        public override string Title => Titles.send_stickers;
        public override string Description => Descriptions.send_stickers;

        public override async Task<bool> Execute(UserEntity user, object[] args = null)
        {
            var task = await DailyTaskDao.GetTaskInfo(user.Id, Id);
            if (task.Progress == 0) return false;
            task.Progress--;
            if (user.Settings[UserSettingsEnum.DailyTaskProgress])
                await MessageController.SendMessage(user,
                    $"{Messages.send_sticker_progress}: {Goal - task.Progress} / {Goal}", addToList: false);
            return task.Progress == 0;
        }

        public override async Task GiveReward(UserEntity user, object[] args = null)
        {
            var userPacks = await UserPacksDao.GetOne(user.Id, 1);
            userPacks.Count++;
        }
    }
}