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
                ? new TimeSpan(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds + 1000)
                : new TimeSpan((DateTime.Now.TimeOfDay.Hours / 4 + 1) * 4, 0, 0);
        
        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            var users = await UserDao.GetAllWhere(user => !user.IsBlocked);
            var settings = await SettingsDao.GetAll();
            foreach (var user in users)
            {
                var cash = await CashDao.GetById(user.Id);
                var stickers = await UserStickerRelationDao.GetListById(user.Id);
                var income = await cash.CalculateIncome(stickers);
                try {
                    if (settings[user.Id][UserSettingsEnum.PiggyBankCapacity])
                        await MessageController.SendMessage(user, 
                            $"{Messages.uncollected_income}: {income} / {cash.MaxCapacity} {Text.coin}", addToList: false);
                }
                catch {
                    await MessageController.SendMessage(user, 
                        $"{Messages.uncollected_income}: {income} / {cash.MaxCapacity} {Text.coin}", addToList: false);
                }
            }
        }
    }
}