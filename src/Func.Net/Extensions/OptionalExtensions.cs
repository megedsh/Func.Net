using System;
using System.Collections.Generic;
using System.Text;

namespace Func.Net.Extensions
{
    public static class OptionalExtensions
    {
        public static Optional<TVal> GetOptional<TKey, TVal>(this IDictionary<TKey, TVal> dictionary, TKey key)
        {
            if (dictionary.TryGetValue(key, out TVal val))
            {
                return Optional.OfNullable(val);
            }

            return Optional.Empty<TVal>();
        }

        public static Optional<T> GetOptional<T>(this ISet<T> set, T item)
        {
            if (set.Contains(item))
            {
                return Optional.OfNullable(item);
            }

            return Optional.Empty<T>();
        }
    }
}
