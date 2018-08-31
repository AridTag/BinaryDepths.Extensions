using System;
using System.Collections.Generic;

namespace System.Linq
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Determines if an IEnumerable contains only a single element
        /// </summary>
        /// <remarks>
        /// Care should be taken when using this, as some collections will not allow you to get the first element after you have looked at it.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns>True if there is only 1 element</returns>
        public static bool HasSingleElement<T>(this IEnumerable<T> sequence)
        {
            if (!sequence.Any())
                return false;

            return !sequence.Skip(1).Any();
        }
        
        /// <summary>
        /// Gets the object with the largest value selected by selector
        /// </summary>
        /// <remarks>
        /// From StackOverflow http://stackoverflow.com/questions/1101841/linq-how-to-perform-max-on-a-property-of-all-objects-in-a-collection-and-ret
        /// Asked by: theringostarrs http://stackoverflow.com/users/105916/theringostarrs
        /// Answer by: meustrus http://stackoverflow.com/users/710377/meustrus
        /// </remarks>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="selector"></param>
        /// <returns>
        /// The object with the largest value
        /// </returns>
        public static TObject MaxBy<TObject, TR>(this IEnumerable<TObject> sequence, Func<TObject, TR> selector) where TR : IComparable<TR>
        {
            return sequence.Select(t => new Tuple<TObject, TR>(t, selector(t)))
                           .Aggregate((max, next) => next.Item2.CompareTo(max.Item2) > 0 ? next : max).Item1;
        }
        
        /// <summary>
        /// Gets the object that has the smallest value selected by selector
        /// </summary>
        /// <remarks>
        /// From StackOverflow http://stackoverflow.com/questions/1101841/linq-how-to-perform-max-on-a-property-of-all-objects-in-a-collection-and-ret
        /// Asked by: theringostarrs http://stackoverflow.com/users/105916/theringostarrs
        /// Answer by: meustrus http://stackoverflow.com/users/710377/meustrus
        /// </remarks>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="selector"></param>
        /// <returns>The object with the smallest value</returns>
        public static TObject MinBy<TObject, TR>(this IEnumerable<TObject> sequence, Func<TObject, TR> selector) where TR : IComparable<TR>
        {
            return sequence.Select(t => new Tuple<TObject, TR>(t, selector(t)))
                           .Aggregate((max, next) => next.Item2.CompareTo(max.Item2) < 0 ? next : max).Item1;
        }
        
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
