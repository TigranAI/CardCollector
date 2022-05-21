using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class LogsMenu : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.logs_menu;

        protected override async Task Execute()
        {
            var totalGroupsCount =
                await Context.TelegramChats.CountAsync(item =>
                    item.ChatType == ChatType.Group || item.ChatType == ChatType.Supergroup);
            var date = Convert.ToDateTime(CallbackQuery.Data!.Split('=')[1]);
            var message = string.Format(Messages.logs_on_date, date.ToString("dd.MM.yyyy"));
            var logsPage = date.Date == DateTime.Today.Date
                ? await GetCurrentResults()
                : await Context.CountLogs.FindByDate(date);
            message +=
                $"\n{LogsTranslations.PeopleCollectedIncomeOneToThreeTimes}: {logsPage.PeopleCollectedIncomeOneToThreeTimes}" +
                $"\n{LogsTranslations.PeopleCollectedIncomeMoreTimes}: {logsPage.PeopleCollectedIncomeMoreTimes}" +
                $"\n{LogsTranslations.PeopleCompletedDailyTask}: {logsPage.PeopleCompletedDailyTask}" +
                $"\n{LogsTranslations.PeopleSendsStickerOneOrMoreTimes}: {logsPage.PeopleSendsStickerOneOrMoreTimes}" +
                $"\n{LogsTranslations.PeopleDonated}: {logsPage.PeopleDonated}" +
                $"\n{LogsTranslations.PeoplePutsStickerToAuction}: {logsPage.PeoplePutsStickerToAuction}" +
                $"\n{LogsTranslations.InvitedUsers}: {logsPage.InvitedUsers}" +
                $"\n\n{Text.groups}:" +
                $"\n{LogsTranslations.GroupCountWasAdded}: {logsPage.GroupCountWasAdded} / {totalGroupsCount}" +
                $"\n{LogsTranslations.GroupCountWasActive}: {logsPage.GroupCountWasActive}" +
                $"\n{LogsTranslations.RoulettePlayCount}: {logsPage.RoulettePlayCount}" +
                $"\n{LogsTranslations.GroupPrizeCount}: {logsPage.GroupPrizeCount}";
            await User.Messages.EditMessage(message, Keyboard.LogsMenu(date), ParseMode.Html);
        }

        private async Task<CountLogs> GetCurrentResults()
        {
            var leftBorder = DateTime.Today;
            var rightBorder = leftBorder.AddDays(1);
            var todayActivities = await Context.UserActivities
                .Where(item => item.Timestamp >= leftBorder && item.Timestamp < rightBorder)
                .ToListAsync();
            var results = new CountLogs();
            return Functions.CalculateActivityResults(results, todayActivities);
        }
    }
}