using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.TimerTasks;

public class TopUsersAwards : TimerTask
{
    protected override TimeSpan RunAt => Constants.DEBUG
        ? GetStartOfNextEvenMinute()
        : GetStartOfNextEvenWeek();

    private Dictionary<int, TopReward> Rewards = new()
    {
        {0, new TopReward(200, 10000, 10000)},
        {1, new TopReward(50, 5, 2000)},
        {2, new TopReward(10, 1000, 500)},
    };

    private TimeSpan GetStartOfNextEvenMinute()
    {
        var dateTime = DateTime.Now;
        var hour = dateTime.Hour;
        var minute = dateTime.Minute + 2 - dateTime.Minute % 2;
        return new TimeSpan(hour, minute, 0);
    }

    private static TimeSpan GetStartOfNextEvenWeek()
    {
        int AsStartOfMonday(DayOfWeek dayOfWeek) => dayOfWeek is DayOfWeek.Sunday ? 7 : (int) dayOfWeek;
        
        var dateTime = DateTime.Now;
        var cal = CultureInfo.InvariantCulture.Calendar;
        var week = cal.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        var day = 7 - AsStartOfMonday(dateTime.DayOfWeek) + week % 2 * 7;
        return new TimeSpan(day == 0 ? 14 : day, 0, 0, 0);
    }

    protected override async void TimerCallback(object o, ElapsedEventArgs e)
    {
        using (var context = new BotDatabaseContext())
        {
            var topHistory = new TopHistory()
            {
                Exp = await RewardByTop(context, UserStatsDao.GetTopByExp, Text.top_by_exp),
                Tier4 = await RewardByTop(context, UserStatsDao.GetTopByTier4Sticker, Text.top_by_tier_4_sticker),
                Roulette = await RewardByTop(context, UserStatsDao.GetTopByRouletteGames, Text.top_by_roulette_games),
                Ladder = await RewardByTop(context, UserStatsDao.GetTopByLadderGames, Text.top_by_ladder_games),
                Puzzle = await RewardByTop(context, UserStatsDao.GetTopByPuzzleGames, Text.top_by_puzzle_games),
                Gift = await RewardByTop(context, UserStatsDao.GetTopByGiftsReceived, Text.top_by_gifts_received),
                Invite = await RewardByTop(context, UserStatsDao.GetTopByInvitedFriends, Text.top_by_invited_friends)
            };
            await context.TopHistory.AddAsync(topHistory);
            (await context.UsersStats.ToListAsync())
                .Apply(item => item.Reset());
            await context.SaveChangesAsync();
        }
    }

    private async Task<string> RewardByTop(BotDatabaseContext context,
        Func<DbSet<UserStats>, Task<List<UserStats>>> topExpr,
        string topType)
    {
        var top = await topExpr.Invoke(context.UsersStats);
        foreach (var topPosition in top.WithIndex())
        {
            var reward = Rewards[topPosition.index];
            var user = topPosition.item.User;
            await user.Messages.SendMessage(
                string.Format(Messages.top_reward_message, topPosition.index + 1, topType, reward));
            await reward.RewardUser(context, user);
        }

        return string.Join(", ", top.Select(item => item.User.Id));
    }
}