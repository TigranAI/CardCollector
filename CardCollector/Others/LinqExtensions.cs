using System;
using System.Collections.Generic;
using System.Linq;
using CardCollector.Extensions;

namespace CardCollector.Others
{
    public static class LinqExtensions
    {
        public static T Random<T>(this IEnumerable<T> source)
        {
            var list = source.ToList();
            return list[Utilities.Rnd.Next(list.Count)];
        }

        public static T? WeightedRandom<T>(this IEnumerable<T> source, Func<T, int> weightExpression)
        {
            var pool = source.Sum(item => weightExpression.Invoke(item));
            var value = Utilities.Rnd.Next(pool);
            var sum = 0;
            foreach (var item in source)
            {
                var step = weightExpression.Invoke(item);
                if (value >= sum && value < sum + step) return item;
                sum += step;
            }
            return default;
        }
        
        public static IList<T> Shuffle<T>(this IList<T> source)
        {
            var n = source.Count;
            while (n > 1) {
                n--;
                var k = Utilities.Rnd.Next(n + 1);
                var value = source[k];
                source[k] = source[n];
                source[n] = value;
            }
            return source;
        }
    }
}