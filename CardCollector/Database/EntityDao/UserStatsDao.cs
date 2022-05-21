using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao;

public static class UserStatsDao
{
    private static Task<List<UserStats>> GetTop(
        this DbSet<UserStats> table,
        Expression<Func<UserStats, bool>> condition,
        Expression<Func<UserStats, long>> property,
        Expression<Func<UserStats, DateTime>> dateProperty)
    {
        return table
            .Where(condition)
            .OrderByDescending(property)
            .ThenBy(dateProperty)
            .Take(3)
            .Include(item => item.User)
            .ToListAsync();
    }
    
    public static Task<List<UserStats>> GetTopByExp(this DbSet<UserStats> table)
    {
        return table.GetTop(
            item => item.EarnedExp > 0,
            item => item.EarnedExp, 
            item => item.LastExpAccrual);
    }
    
    public static Task<List<UserStats>> GetTopByTier4Sticker(this DbSet<UserStats> table)
    {
        return table.GetTop(
            item => item.Earned4TierStickers > 0,
            item => item.Earned4TierStickers, 
            item => item.Last4TierStickerAccrual);
    }
    
    public static Task<List<UserStats>> GetTopByRouletteGames(this DbSet<UserStats> table)
    {
        return table.GetTop(
            item => item.RouletteGames > 0,
            item => item.RouletteGames, 
            item => item.LastRouletteGame);
    }
    
    public static Task<List<UserStats>> GetTopByLadderGames(this DbSet<UserStats> table)
    {
        return table.GetTop(
            item => item.LadderGames > 0,
            item => item.LadderGames, 
            item => item.LastLadderGame);
    }
    
    public static Task<List<UserStats>> GetTopByPuzzleGames(this DbSet<UserStats> table)
    {
        return table.GetTop(
            item => item.PuzzleGames > 0,
            item => item.PuzzleGames, 
            item => item.LastPuzzleGame);
    }
    
    public static Task<List<UserStats>> GetTopByGiftsReceived(this DbSet<UserStats> table)
    {
        return table.GetTop(
            item => item.GiftsReceived > 0,
            item => item.GiftsReceived, 
            item => item.LastReceivedGift);
    }
    
    public static Task<List<UserStats>> GetTopByInvitedFriends(this DbSet<UserStats> table)
    {
        return table.GetTop(
            item => item.FriendsInvited > 0,
            item => item.FriendsInvited, 
            item => item.LastInvitedFriend);
    }
}