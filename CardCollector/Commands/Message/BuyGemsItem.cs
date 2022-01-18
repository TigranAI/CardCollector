using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    public class BuyGemsItem : MessageCommand
    {
        protected override string CommandText => Command.buy_gems_item;

        public override async Task Execute()
        {
            var amount = Update.Message.SuccessfulPayment!.TotalAmount;
            var gemsCount = amount / 69;
            User.Cash.Gems += gemsCount;
            await MessageController.EditMessage(User, Messages.thanks_for_buying_gems);
            if (User.Settings[UserSettingsEnum.ExpGain])
                await MessageController.SendMessage(User,
                    $"{Messages.you_gained} {gemsCount * 2} {Text.exp} {Messages.for_buy_gems}", Keyboard.BackKeyboard);
            await User.GiveExp(gemsCount * 2);
            PaymentDao.AddPayment(Update.Message.SuccessfulPayment, User, Update.Id);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return update.Message?.Type == MessageType.SuccessfulPayment &&
                   update.Message.SuccessfulPayment!.InvoicePayload.Equals(CommandText);
        }

        public BuyGemsItem()
        {
        }

        public BuyGemsItem(UserEntity user, Update update) : base(user, update)
        {
        }

        private static PeopleDonated _info = PeopleDonated.Build().Result;

        public override async Task AfterExecute()
        {
            if (!_info.Actual())
            {
                _info.WriteResults();
                _info = await PeopleDonated.Build();
            }

            _info.Add(User.Id);
        }

        private class PeopleDonated
        {
            private DateTime _infoDate;
            private CountLogs _logsEntity;
            private HashSet<long> People = new();

            public static async Task<PeopleDonated> Build()
            {
                var result = new PeopleDonated();
                result._infoDate = DateTime.Today;
                result._logsEntity = await CountLogsDao.Get(result._infoDate);
                return result;
            }

            public bool Actual()
            {
                return _infoDate.Equals(DateTime.Today);
            }

            public void Add(long userId)
            {
                People.Add(userId);
            }

            public void WriteResults()
            {
                _logsEntity.PeopleDonated += People.Count;
            }
        }

        public static void WriteLogs()
        {
            _info.WriteResults();
        }
    }
}