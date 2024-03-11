using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqMethods
{
    public class LinqMethodsOnIntegers
    {
        public static IEnumerable<IEnumerable<int>> CompareSublistIntegersSum(int[] integers, int k)
        {
            return LinqMethodsOnStrings.GetSubsequences(integers).Where(sublist => sublist.Sum() <= k);
        }

        public static IEnumerable<IEnumerable<int>> Combinations(int n, int k)
        {
            var combinations = new[] { Enumerable.Empty<int>() };

            return GetCombinations(n, combinations).Where(combination => combination.Sum() == k);
        }

        private static IEnumerable<IEnumerable<int>> GetCombinations(int n, IEnumerable<IEnumerable<int>> combinations)
        {
            return Enumerable.Range(1, n).Aggregate(combinations, (x, number) => x.SelectMany(comb => new[]
                {
                    comb.Append(number),
                    comb.Append(-number)
                }
            ));
        }

        public static IEnumerable<(int a, int b, int c)> PythagoreanTheorem(int[] randomNumbers)
        {
            var sortedNumbers = randomNumbers.Select(x => x).OrderBy(x => x);
            return sortedNumbers.SelectMany((a, i) => sortedNumbers.Skip(i + 1)
                       .SelectMany((b, j) => sortedNumbers.Skip(i + j + 2)
                            .Where(c => OrderTuple(a, b, c))
                                 .Select(c => (a, b, c))));
        }

        private static bool OrderTuple(int a, int b, int c)
        {
            return a * a + b * b == c * c ||
                   b * b + c * c == a * a ||
                   a * a + c * c == b * b;
        }
    }
}
