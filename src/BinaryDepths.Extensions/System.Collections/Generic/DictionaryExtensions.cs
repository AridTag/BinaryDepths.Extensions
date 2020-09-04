using System.Collections.Generic;

namespace BinaryDepths.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds the specified value with the specified key to the dictionary or updates the value at that key if it already exists
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
                return;
            }

            dict.Add(key, value);
        }
    }
}
