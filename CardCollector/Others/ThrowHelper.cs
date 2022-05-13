using System;

namespace CardCollector.Others
{
    internal static class ThrowHelper
    {
        public static void CheckNonNull(object? value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
        }
    }
}