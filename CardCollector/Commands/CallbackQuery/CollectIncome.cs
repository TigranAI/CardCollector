using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CollectIncome : CallbackQueryCommand
    {
        protected override string CommandText => Command.collect_income;
        private static PeopleCollectedIncomeInfo _info = PeopleCollectedIncomeInfo.Build().Result;

        public override async Task Execute()
        {
            var result = await User.Cash.Payout(User.Stickers);
            await MessageController.AnswerCallbackQuery(User, CallbackQueryId, 
                $"{Messages.you_collected} {result}{Text.coin} " +
                $"\n\n{Messages.your_cash}: {User.Cash.Coins}{Text.coin} {User.Cash.Gems}{Text.gem}", true);
            var expGoal = (await LevelDao.GetLevel(User.CurrentLevel.Level + 1))?.LevelExpGoal.ToString() ?? "∞";
            var packsCount = await UserPacksDao.GetCount(User.Id);
            /* Отправляем сообщение */
            await MessageController.EditMessage(User, 
                $"{User.Username}" +
                $"\n{Messages.coins}: {User.Cash.Coins}{Text.coin}" +
                $"\n{Messages.gems}: {User.Cash.Gems}{Text.gem}" +
                $"\n{Messages.level}: {User.CurrentLevel.Level}" +
                $"\n{Messages.current_exp}: {User.CurrentLevel.CurrentExp} / {expGoal}" +
                $"\n{Messages.cash_capacity}: {User.Cash.MaxCapacity}{Text.coin}",
                Keyboard.GetProfileKeyboard(packsCount));
        }

        public override async Task AfterExecute()
        {
            if (!_info.Actual())
            {
                _info.WriteResults();
                _info = await PeopleCollectedIncomeInfo.Build();
            }
            _info.Add(User.Id);
        }

        public CollectIncome() { }
        public CollectIncome(UserEntity user, Update update) : base(user, update) { }
        
        private class PeopleCollectedIncomeInfo
        {
            private DateTime infoDate;
            private CountLogs logsEntity;
            private Dictionary<long, int> UserCollectCount = new ();

            public static async Task<PeopleCollectedIncomeInfo> Build()
            {
                var result = new PeopleCollectedIncomeInfo();
                result.infoDate = DateTime.Today;
                result.logsEntity = await CountLogsDao.Get(result.infoDate);
                return result;
            }

            public bool Actual()
            {
                return infoDate.Equals(DateTime.Today);
            }

            public void Add(long userId)
            {
                if (!UserCollectCount.ContainsKey(userId))
                    UserCollectCount.Add(userId, 1);
                else UserCollectCount[userId]++;
            }

            public void WriteResults()
            {
                var oneToThreeTimes = UserCollectCount.Values.Count(item => item < 4);
                logsEntity.PeopleCollectedIncomeOneToThreeTimes += oneToThreeTimes;
                logsEntity.PeopleCollectedIncomeMoreTimes += UserCollectCount.Count - oneToThreeTimes;
            }
        }

        public static void WriteLogs()
        {
            _info.WriteResults();
        }
    }
}