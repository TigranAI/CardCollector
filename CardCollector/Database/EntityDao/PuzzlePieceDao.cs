using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Extensions;
using CardCollector.Others;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao;

public static class PuzzlePieceDao
{
    public static async Task<PuzzlePiece> FindById(this DbSet<PuzzlePiece> table, long pieceId)
    {
        return await table.FirstAsync(item => item.Id == pieceId);
    }
    
    public static async Task<List<PuzzlePiece>> GetAllByPieceCount(
        this DbSet<PuzzlePiece> table,
        int pieceCount)
    {
        return await table
            .Where(piece => piece.PieceCount == pieceCount)
            .ToListAsync();
    }

    public static async Task<List<PuzzlePiece>> GetRandomWithSelectedPiece(
        this DbSet<PuzzlePiece> table,
        long stickerId,
        int order)
    {
        var piece = await table.SingleAsync(item => item.Sticker.Id == stickerId && item.Order == order);
        var pieces = await table
            .OrderByRandom()
            .Take(6)
            .ToListAsync();
        if (!pieces.Any(item => item.Id == piece.Id)) pieces[Utilities.Rnd.Next(6)] = piece;
        return pieces.Shuffle().ToList();
    }
}