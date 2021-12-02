using System;
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
        private static PeopleCompletedTask info = PeopleCompletedTask.Build().Result;

        public override async Task<bool> Execute(UserEntity user, object[] args = null)
        {
            var task = await DailyTaskDao.GetTaskInfo(user.Id, Id);
            if (task.Progress == 0) return false;
            task.Progress--;
            if (user.Settings[UserSettingsEnum.DailyTaskProgress])
                user.MessagesId.DailyTaskProgressId = 
                await MessageController.DeleteAndSend(user, user.MessagesId.DailyTaskProgressId,
                    $"{Messages.send_sticker_progress}: {Goal - task.Progress} / {Goal}");
            if (task.Progress == 0)
            {
                if (!info.Actual())
                {
                    info.WriteResults();
                    info = await PeopleCompletedTask.Build();
                }
                info.Increase();
                return true;
            }
            return false;
        }

        public override async Task GiveReward(UserEntity user, object[] args = null)
        {
            var userPacks = await UserPacksDao.GetOne(user.Id, 1);
            userPacks.Count++;
        }

        private class PeopleCompletedTask
        {
            private DateTime infoDate;
            private CountLogs logsEntity;
            private int count = 0;

            public static async Task<PeopleCompletedTask> Build()
            {
                var result = new PeopleCompletedTask();
                result.infoDate = DateTime.Today;
                result.logsEntity = await CountLogsDao.Get(result.infoDate);
                return result;
            }

            public bool Actual()
            {
                return infoDate.Equals(DateTime.Today);
            }

            public void Increase()
            {
                count++;
            }

            public void WriteResults()
            {
                logsEntity.PeopleCompletedDailyTask += count;
            }
        }

        public static void WriteLogs()
        {
            info.WriteResults();
        }
    }
}