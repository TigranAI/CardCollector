using System.Collections.Generic;
using CardCollector.Commands;

namespace CardCollector.Others
{
    public class HandlerEqualityComparer : IEqualityComparer<HandlerModel>
    {
        public bool Equals(HandlerModel? x, HandlerModel? y)
        {
            return x?.GetType() == y?.GetType() && x != null;
        }

        public int GetHashCode(HandlerModel obj)
        {
            return obj.GetType().GetHashCode();
        }
    }
}