using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public abstract class SendStickerHandler : ChosenInlineResultHandler
    {
        private static PeopleSendsSticker info = PeopleSendsSticker.Build().Result;
        
        public override async Task AfterExecute()
        {
            if (!info.Actual())
            {
                info.WriteResults();
                info = await PeopleSendsSticker.Build();
            }
            info.Add(User.Id);
        }
        
        private class PeopleSendsSticker
        {
            private DateTime infoDate;
            private CountLogs logsEntity;
            private HashSet<long> People = new();

            public static async Task<PeopleSendsSticker> Build()
            {
                var result = new PeopleSendsSticker();
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
                logsEntity.PeopleSendsStickerOneOrMoreTimes += People.Count;
            }
        }

        public static void WriteLogs()
        {
            info.WriteResults();
        }

        protected SendStickerHandler(UserEntity user, Update update) : base(user, update) { }
    }
}