using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Database.Entity;

public class UserStats
{
    [Key, ForeignKey("id")]
    public virtual User User { get; set; }
    
    public long EarnedExp { get; set; }
    public DateTime LastExpAccrual { get; set; }
    
    public int Earned4TierStickers { get; set; }
    public DateTime Last4TierStickerAccrual { get; set; }
    
    public int RouletteGames { get; set; }
    public DateTime LastRouletteGame { get; set; }
    
    public int LadderGames { get; set; }
    public DateTime LastLadderGame { get; set; }
    
    public int PuzzleGames { get; set; }
    public DateTime LastPuzzleGame { get; set; }
    
    public int GiftsReceived { get; set; }
    public DateTime LastReceivedGift { get; set; }
    
    public int FriendsInvited { get; set; }
    public DateTime LastInvitedFriend { get; set; }

    public void GainExp(long count)
    {
        EarnedExp += count;
        LastExpAccrual = DateTime.Now;
    }

    public void Gain4TierSticker(int count)
    {
        Earned4TierStickers += count;
        Last4TierStickerAccrual = DateTime.Now;
    }

    public void IncreaseRouletteGames()
    {
        RouletteGames++;
        LastRouletteGame = DateTime.Now;
    }

    public void IncreaseLadderGames()
    {
        LadderGames++;
        LastLadderGame = DateTime.Now;
    }

    public void IncreasePuzzleGames()
    {
        PuzzleGames++;
        LastPuzzleGame = DateTime.Now;
    }

    public void IncreaseGiftsReceived()
    {
        GiftsReceived++;
        LastReceivedGift = DateTime.Now;
    }

    public void IncreaseFriendsInvited()
    {
        FriendsInvited++;
        LastInvitedFriend = DateTime.Now;
    }

    public void Reset()
    {
        EarnedExp = 
        Earned4TierStickers = 
        RouletteGames = 
        LadderGames = 
        PuzzleGames = 
        GiftsReceived = 
        FriendsInvited = 0;
        
        LastExpAccrual = 
        Last4TierStickerAccrual = 
        LastRouletteGame = 
        LastLadderGame = 
        LastPuzzleGame = 
        LastReceivedGift = 
        LastInvitedFriend = DateTime.Now;
    }
}