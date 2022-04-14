using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CardCollector.Others
{
    public class CollectionComparer<T> : ValueComparer<List<T>>
    {
        private static Expression<Func<List<T>?, List<T>?, bool>> ListEqualsExpression = (l1, l2) =>
            l1 != null
            && l2 != null
            && (ReferenceEquals(l1, l2) || l1.SequenceEqual(l2));

        private static Expression<Func<List<T>, int>> ListHashCodeExpression = (l) =>
            l.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()));

        public CollectionComparer() : base(ListEqualsExpression, ListHashCodeExpression, l => l.ToList())
        {
        }
    }
}