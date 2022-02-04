using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Commands.Message;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ConfirmSelling : CallbackQueryHandler
    {
        protected override string CommandText => Command.confirm_selling;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            var collectionModule = User.Session.GetModule<CollectionModule>();
            if (collectionModule.SellPrice <= 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.cant_sell_zero, true);
            else
            {
                EnterGemsPrice.RemoveFromQueue(User.Id);
                User.Stickers[collectionModule.SelectedSticker.Md5Hash].Count -= collectionModule.Count;
                AuctionController.SellCard(User.Id, collectionModule.SelectedSticker.Id, collectionModule.SellPrice,
                    collectionModule.Count);
                await MessageController.EditMessage(User, Messages.successfully_selling);
            }
        }
        
        public ConfirmSelling(UserEntity user, Update update) : base(user, update){}
        
        private static PeoplePutsStickerOnAuction info = PeoplePutsStickerOnAuction.Build().Result;
        
        public override async Task AfterExecute()
        {
            if (!info.Actual())
            {
                info.WriteResults();
                info = await PeoplePutsStickerOnAuction.Build();
            }
            info.Add(User.Id);
        }
        
        private class PeoplePutsStickerOnAuction
        {
            private DateTime infoDate;
            private CountLogs logsEntity;
            private HashSet<long> People = new();

            public static async Task<PeoplePutsStickerOnAuction> Build()
            {
                var result = new PeoplePutsStickerOnAuction();
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
                logsEntity.PeoplePutsStickerToAuction += People.Count;
            }
        }

        public static void WriteLogs()
        {
            info.WriteResults();
        }
    }
}