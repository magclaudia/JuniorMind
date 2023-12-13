using System;

namespace Linq
{
    public static class LinqMethods
    {
        public static bool All<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate)
        {
            ThrowArgumentNullException(source);
            ThrowArgumentNullException(predicate);
            foreach (var item in source!)
            {
                if (!predicate!(item))
                {
                   return false;
                }
            }

            return true;
        }

        public static bool Any<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate)
        {
            ThrowArgumentNullException(source);
            ThrowArgumentNullException(predicate);
            foreach (var item in source!)
            {
                if (predicate!(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static TSource First<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate)
        {
            ThrowArgumentNullException(source);
            ThrowArgumentNullException(predicate);
            foreach (var item in source!)
            {
                if (predicate!(item))
                {
                    return item;
                }
            }

            throw new InvalidOperationException("No items matched the predicate or The source sequence is empty.");
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource>? source, Func<TSource, TResult>? selector)
        {
            ThrowArgumentNullException(source);
            ThrowArgumentNullException(selector);
            foreach (var item in source!)
            {
                yield return selector!(item);
            }
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource>? source, Func<TSource, IEnumerable<TResult>>? selector)
        {
            ThrowArgumentNullException(source);
            ThrowArgumentNullException(selector);
            foreach (var item in source!)
            {
                foreach (var result in selector!(item))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate)
        {
            ThrowArgumentNullException(source);
            ThrowArgumentNullException(predicate);
            foreach (var item in source!)
            {
                if (predicate!(item))
                {
                   yield return item;
                }
            }
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource>? source, Func<TSource, TKey>? keySelector, Func<TSource, TElement>? elementSelector)
        {
            ThrowArgumentNullException(source);
            ThrowArgumentNullException(keySelector);
            ThrowArgumentNullException (elementSelector);
            var dictionary = new Dictionary<TKey, TElement>();
            foreach (var item in source!)
            {
                var key = keySelector!(item);
                if (key == null)
                {
                    ThrowArgumentNullException(key);

                }

                try
                {
                     dictionary.Add(keySelector(item), elementSelector!(item));
                }
                catch
                {
                     throw new ArgumentException("Source contains one or more duplicate keys.");
                }
            }

            return dictionary;
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>( this IEnumerable<TFirst>? first, IEnumerable<TSecond>? second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            ThrowArgumentNullException(first);
            ThrowArgumentNullException(second);
            using (var firstEnumerator = first!.GetEnumerator())
            {
                using (var secondEnumerator = second!.GetEnumerator())
                {
                    while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
                    {
                          yield return resultSelector(firstEnumerator.Current, secondEnumerator.Current);
                    }
                }
            }
        }

        public static TAccumulate Aggregate<TSource, TAccumulate>( this IEnumerable<TSource>? source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate>? func)
        {
            ThrowArgumentNullException(source);
            ThrowArgumentNullException(func);
            foreach (var item in source!) 
            {
                seed = func!(seed, item);
            }

            return seed;
        }

        private static void ThrowArgumentNullException<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("{0} is a null reference.", nameof(item));
            }
        }
    }
}
