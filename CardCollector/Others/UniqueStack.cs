using System.Collections.Generic;
using System.Linq;

namespace CardCollector.Others
{
    public class UniqueStack<T> : Stack<T>
    {
        private IEqualityComparer<T>? _comparer;

        public new void Push(T item)
        {
            if (_comparer != null && !this.Contains(item, _comparer)) base.Push(item);
            else if (_comparer == null && !Contains(item)) base.Push(item);
        }

        public void SetComparer(IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public UniqueStack()
        {
        }

        public UniqueStack(IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
        }
    }
}