using System;
using System.Collections.Generic;
using System.Text;

namespace Func.Net.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Validations.RequireNonNull(source, nameof(source));
            Validations.RequireNonNull(action, nameof(action));
            foreach (T element in source)
                action(element);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T,int> action)
        {
            Validations.RequireNonNull(source, nameof(source));
            Validations.RequireNonNull(action, nameof(action));
            int index = -1;
            foreach (T element in source)
            {
                checked
                {
                    index++;
                }

                action(element,index);
            }
        }

        public static IEnumerable<T> Peek<T>(this IEnumerable<T> source, Action<T> action)
        {
            Validations.RequireNonNull(source, nameof(source));
            Validations.RequireNonNull(action, nameof(action));

            return iterator();

            IEnumerable<T> iterator()
            {
                foreach (var item in source)
                {
                    action(item);
                    yield return item;
                }
            }
        }

        public static HashSet<T> ToHashSet<T>(
            this IEnumerable<T> source,
            IEqualityComparer<T> comparer = null)
        {
            Validations.RequireNonNull(source, nameof(source));
            return new HashSet<T>(source, comparer);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TElement, TElement, TElement> duplicateResolver)
        {
            Validations.RequireNonNull(source, nameof(source));
            Validations.RequireNonNull(keySelector, nameof(keySelector));
            Validations.RequireNonNull(elementSelector, nameof(elementSelector));
            Validations.RequireNonNull(duplicateResolver, nameof(duplicateResolver));
            var dictionary = new Dictionary<TKey, TElement>();
            foreach (TSource source1 in source)
            {
                if (dictionary.TryGetValue(keySelector(source1), out TElement existing))
                {
                    dictionary[keySelector(source1)] = duplicateResolver(existing, elementSelector(source1));
                }
                else
                {
                    dictionary[keySelector(source1)] = elementSelector(source1);
                }
            }

            return dictionary;
        }
    }
}

