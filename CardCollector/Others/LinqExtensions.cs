using System;
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

        public static T? WeightedRandom<T>(this IEnumerable<T> source, Func<T, int> weightExpression)
        {
            var pool = source.Sum(item => weightExpression.Invoke(item));
            var value = Utilities.rnd.Next(pool);
            var sum = 0;
            foreach (var item in source)
            {
                var step = weightExpression.Invoke(item);
                if (value >= sum && value < sum + step) return item;
                sum += step;
            }
            return default;
        }
    }
}