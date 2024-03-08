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

        public static IEnumerable<IEnumerable<int>> PythagoreanTheorem(int[] randomNumbers)
        {
            var pairs = randomNumbers.SelectMany((number1, index1) => randomNumbers.Skip(index1 + 1)
                       .SelectMany((number2, index2) => randomNumbers.Skip(index1 + index2 + 2)
                                 .Select(number3 => new[] { number1, number2, number3 })));


            var pythagoreanCombinations = pairs.Where(combinations =>
            {
                var orderCombinations = combinations.OrderBy(p => p).ToArray();
                return orderCombinations[0] * orderCombinations[0] + orderCombinations[1] * orderCombinations[1] == orderCombinations[2] * orderCombinations[2];

            });

            return pythagoreanCombinations.Select(triplet => triplet.OrderBy(x => x));
        }
    }
}
