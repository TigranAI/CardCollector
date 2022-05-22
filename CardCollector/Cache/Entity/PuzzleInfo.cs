using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;

namespace CardCollector.Cache.Entity;

public class PuzzleInfo
{
    public int GamesToday;
    public long StickerId = -1;
    public long CreatorId { get; set; }
    public int MessageId { get; set; } = -1;
    public bool SuperGame { get; set; }

    public int Turn;
    public List<long> Players = new();

    public async Task Start(long chatId)
    {
        if (StickerId != -1) return;
        StickerId = 0;
        using (var context = new BotDatabaseContext())
        {
            var chat = await context.TelegramChats.FindById(chatId);

            await context.SaveChangesAsync();
        }
    }

    public bool TryMakeAMove(PuzzlePiece piece)
    {
        if (Turn == 0) StickerId = piece.Sticker.Id;
        if (piece.Order != Turn || StickerId != piece.Sticker.Id) return false;
        Turn++;
        return true;
    }

    public int GetOrder(long userId)
    {
        return GetPlayers().IndexOf(userId);
    }

    public void Reset()
    {
        StickerId = -1;
        CreatorId = 0;
        SuperGame = false;
        Turn = 0;
        MessageId = -1;
        Players.Clear();
    }

    public void EndGame()
    {
        GamesToday++;
        Reset();
    }

    public List<long> GetPlayers()
    {
        var result = new List<long>() {CreatorId};
        result.AddRange(Players);
        return result;
    }

    public bool IsSuperGame()
    {
        var chance = Utilities.Rnd.Next(1000);
        if (chance < 25) SuperGame = true;
        return SuperGame;
    }

    public bool IsEndOfGame()
    {
        return Turn - 1 == Players.Count;
    }
}