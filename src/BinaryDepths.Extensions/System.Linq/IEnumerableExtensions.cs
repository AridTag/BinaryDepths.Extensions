using System;
using System.Collections.Generic;

namespace System.Linq
{
    public static class IEnumerableExtensions
    {
        public static bool EqualsAny<T>(this T @this, params T[] others)
        {
            return others.Contains(@this);
        }

        public static bool EqualsAny<T>(this T @this, IEqualityComparer<T> comparer, params T[] others)
        {
            return others.Contains(@this, comparer);
        }
    }
}
