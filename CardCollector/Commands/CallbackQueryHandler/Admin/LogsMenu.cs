using System;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class LogsMenu : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.logs_menu;
        
        protected override async Task Execute()
        {
            var date = Convert.ToDateTime(CallbackQuery.Data!.Split('=')[1]);
            var message = string.Format(Messages.logs_on_date, date.ToString("dd.MM.yyyy"));
            var logsPage = await Context.CountLogs.FindByDate(date);
            message += $"\n{LogsTranslations.PCIOTTT}: {logsPage.PeopleCollectedIncomeOneToThreeTimes}" +
                       $"\n{LogsTranslations.PCIMT}: {logsPage.PeopleCollectedIncomeMoreTimes}" +
                       $"\n{LogsTranslations.PCDT}: {logsPage.PeopleCompletedDailyTask}" +
                       $"\n{LogsTranslations.PSSOOMT}: {logsPage.PeopleSendsStickerOneOrMoreTimes}" +
                       $"\n{LogsTranslations.PD}: {logsPage.PeopleDonated}" +
                       $"\n{LogsTranslations.PPSTA}: {logsPage.PeoplePutsStickerToAuction}";
            await User.Messages.EditMessage(User, message, Keyboard.LogsMenu(date), ParseMode.Html);
        }

        public LogsMenu(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}