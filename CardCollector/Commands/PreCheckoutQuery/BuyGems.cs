using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector.Commands.PreCheckoutQuery
{
    public class BuyGems : PreCheckoutQueryCommand
    {
        protected override string CommandText => Command.buy_gems_item;

        public override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQueryId);
            var gemsCount = 50 * Amount / 100;
            User.Cash.Gems += gemsCount;
            await MessageController.EditMessage(User, Messages.thanks_for_buying_gems);
            if (User.Settings[UserSettingsEnum.ExpGain])
                await MessageController.SendMessage(User, 
                    $"{Messages.you_gained} {gemsCount * 2} {Text.exp} {Messages.for_buy_gems}");
            await User.GiveExp(gemsCount * 2);
        }

        public BuyGems() { }
        public BuyGems(UserEntity user, Update update) : base(user, update) { }
        
        private static PeopleDonated info = PeopleDonated.Build().Result;
        
        public override async Task AfterExecute()
        {
            if (!info.Actual())
            {
                info.WriteResults();
                info = await PeopleDonated.Build();
            }
            info.Add(User.Id);
        }
        
        private class PeopleDonated
        {
            private DateTime infoDate;
            private CountLogs logsEntity;
            private HashSet<long> People = new();

            public static async Task<PeopleDonated> Build()
            {
                var result = new PeopleDonated();
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
                People.Add(userId);
            }

            public void WriteResults()
            {
                logsEntity.PeopleDonated += People.Count;
            }
        }

        public static void WriteLogs()
        {
            info.WriteResults();
        }
    }
}