using System.Collections.Generic;
using System.Linq;
using CardCollector.Database.Entity;

namespace CardCollector.Extensions.Database.Entity;

public static class UserPackExtensions
{
    public static List<UserPacks> FindNextSkipRandom(this IEnumerable<UserPacks> table, int offset, int count, bool exclusive)
    {
        return table
            .Where(item => item.Pack.Id != 1 && item.Pack.IsExclusive == exclusive && item.Count > 0)
            .Skip(offset)
            .Take(count)
            .ToList();
    }
    
    public static List<UserPacks> FindNextSkipRandom(this IEnumerable<UserPacks> table, int offset, int count)
    {
        return table
            .Where(item => item.Pack.Id != 1 && item.Count > 0)
            .Skip(offset)
            .Take(count)
            .ToList();
    }
}