using System.Collections.Generic;
using System.Linq;

namespace CardCollector.Others
{
    public static class LinqExtensions
    {
        public static T Random<T>(this IEnumerable<T> source)
        {
            var list = source.ToList();
            return list[Utilities.rnd.Next(list.Count)];
        }
    }
}