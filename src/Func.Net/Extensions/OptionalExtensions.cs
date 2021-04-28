using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Optional<T> OptionalFirst<T>(this IEnumerable<T> source)
        {
            Validations.RequireNonNull(source, nameof(source));
            if (source is IList<T> sourceList)
            {
                if (sourceList.Count > 0)
                    return Optional.OfNullable(sourceList[0]);
            }
            else
            {
                foreach (T x1 in source)
                {
                    return Optional.OfNullable(x1);
                }
            }

            return Optional<T>.Empty();
        }

        public static Optional<T> OptionalFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            Validations.RequireNonNull(source, nameof(source));
            Validations.RequireNonNull(predicate, nameof(predicate));

            foreach (T e in source)
            {
                if (predicate(e))
                    return Optional.Of(e);
            }

            return Optional<T>.Empty();
        }

        public static IEnumerable<T> SelectWherePresent<T>(this IEnumerable<Optional<T>> source)
        {
            Validations.RequireNonNull(source, nameof(source));
            return source.Where(o => o.IsPresent).Select(o => o.Get());
        }

        public static Optional<T> ToOptional<T>(this T? nullable)
            where T : struct
        {
            return nullable.HasValue?Optional.Of(nullable.Value):Optional<T>.Empty();
        }

        public static T OrElse<T>(this T? nullable, T value)
            where T : struct
        {
            return nullable ?? value;
        }
    }
}
