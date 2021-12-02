using System;
using System.Timers;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.TimerTasks
{
    public class PiggyBankAlert : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG
                ? new TimeSpan(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes + 1, 0)
                : new TimeSpan((DateTime.Now.TimeOfDay.Hours / 4 + 1) * 4, 0, 0);
        
        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            var users = await UserDao.GetAllWhere(user => !user.IsBlocked);
            var settings = await SettingsDao.GetAll();
            var messages = await UserMessagesDao.GetAll();
            foreach (var user in users)
            {
                var cash = await CashDao.GetById(user.Id);
                var stickers = await UserStickerRelationDao.GetListById(user.Id);
                var income = await cash.CalculateIncome(stickers);
                try {
                    if (settings[user.Id][UserSettingsEnum.PiggyBankCapacity])
                        messages[user.Id].CollectIncomeId = 
                        await MessageController.DeleteAndSend(user, messages[user.Id].CollectIncomeId,
                            $"{Messages.uncollected_income}: {income} / {cash.MaxCapacity} {Text.coin}");
                }
                catch {
                    messages[user.Id].CollectIncomeId = 
                        await MessageController.DeleteAndSend(user, messages[user.Id].CollectIncomeId,
                            $"{Messages.uncollected_income}: {income} / {cash.MaxCapacity} {Text.coin}");
                }
            }
        }
    }
}