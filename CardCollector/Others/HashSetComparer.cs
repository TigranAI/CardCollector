using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardCollector.Others
{
    public class HashSetComparer<T> : ValueComparer<HashSet<T>>
    {
        private static Expression<Func<HashSet<T>?, HashSet<T>?, bool>> ListEqualsExpression = (l1, l2) =>
            l1 != null
            && l2 != null
            && (ReferenceEquals(l1, l2) || l1.SequenceEqual(l2));

        private static Expression<Func<HashSet<T>, int>> ListHashCodeExpression = (l) =>
            l.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()));

        public HashSetComparer() : base(ListEqualsExpression, ListHashCodeExpression, l => l.ToHashSet())
        {
        }
    }
}