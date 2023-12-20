using System;

namespace Linq
{
    public static class LinqMethods
    {
        public static bool All<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate)
        {
            ThrowArgumentNullException(source, "source");
            ThrowArgumentNullException(predicate, "predicate");
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
            ThrowArgumentNullException(source, "source");
            ThrowArgumentNullException(predicate, "predicate");
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
            ThrowArgumentNullException(source, "source");
            ThrowArgumentNullException(predicate, "predicate");
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
            ThrowArgumentNullException(source, "source");
            ThrowArgumentNullException(selector, "selector");
            foreach (var item in source!)
            {
                yield return selector!(item);
            }
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource>? source, Func<TSource, IEnumerable<TResult>>? selector)
        {
            ThrowArgumentNullException(source, "source");
            ThrowArgumentNullException(selector, "selector");
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
            ThrowArgumentNullException(source, "source");
            ThrowArgumentNullException(predicate, "predicate");
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
            ThrowArgumentNullException(source, "source");
            ThrowArgumentNullException(keySelector, "keySelector");
            ThrowArgumentNullException(elementSelector, "elementSelector");
            var dictionary = new Dictionary<TKey, TElement>();
            foreach (var item in source!)
            {
                var key = keySelector!(item);
                if (key == null)
                {
                    ThrowArgumentNullException(key, "key");

                }

                dictionary.Add(keySelector(item), elementSelector!(item));
            }

            return dictionary;
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst>? first, IEnumerable<TSecond>? second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            ThrowArgumentNullException(first, "first");
            ThrowArgumentNullException(second, "second");
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

        public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource>? source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate>? func)
        {
            ThrowArgumentNullException(source, "source");
            ThrowArgumentNullException(func, "func");
            var accumulate = seed;
            foreach (var item in source!)
            {
                accumulate = func!(accumulate, item);
            }

            return accumulate;
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            ThrowArgumentNullException(outer, "outer");
            ThrowArgumentNullException(inner, "inner");
            ThrowArgumentNullException(outerKeySelector, "outerKeySelector");
            ThrowArgumentNullException(innerKeySelector, "innerKeySelector");
            ThrowArgumentNullException(resultSelector, "resultSelector");
            foreach (var outerElement in outer)
            {
                foreach (var innerElement in inner)
                {
                    if (outerKeySelector(outerElement).Equals(innerKeySelector(innerElement)))
                    {
                        yield return resultSelector(outerElement, innerElement);
                    }
                }
            }
        }

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            ThrowArgumentNullException(source, "source");
            var hash = new HashSet<TSource>(comparer);
            foreach (var item in source)
            {
                if (hash.Add(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            ThrowArgumentNullException(first, "first");
            ThrowArgumentNullException(second, "second");
            var hash = new HashSet<TSource>(comparer);
            foreach (var firstListElements in first)
            {
                if (hash.Add(firstListElements))
                {
                    yield return firstListElements;
                }

                if (firstListElements.Equals(first.Last()))
                {
                    foreach (var secondListElements in second)
                    {
                        if (hash.Add(secondListElements))
                        {
                            yield return secondListElements;
                        }
                    }

                    break;
                }
            }
        }

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            ThrowArgumentNullException(first, "first");
            ThrowArgumentNullException(second, "second");
            foreach (var firstListElements in first)
            {
                foreach(var secondListElements in second)
                {
                    if (comparer.Equals(firstListElements, secondListElements))
                    {
                        yield return firstListElements;
                    }
                }
            }
        }

        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            ThrowArgumentNullException(first, "first");
            ThrowArgumentNullException(second, "second");
            foreach (var firstListElements in first)
            {
                if (!second.Contains(firstListElements))
                {
                   yield return firstListElements;
                }
            }
        }

        private static void ThrowArgumentNullException<T>(T item, string parameterName)
        {
            if (item == null)
            {
                throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null.");
            }
        }
    }
}