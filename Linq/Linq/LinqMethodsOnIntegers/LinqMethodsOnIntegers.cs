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
            var resultList = new List<List<int>>();
            var combinations = new List<int>();
            GetListsOfCombinations(n, k, combinations, resultList);
            return resultList;
        }

        private static void GetListsOfCombinations(int n, int k, List<int> combinations, List<List<int>> resultList)
        {
            if (combinations.Count == n)
            {
                if (combinations.Sum() == k)
                {
                    resultList.Add(new List<int>(combinations));
                }

                return;
            }

            combinations.Add(combinations.Count + 1);
            GetListsOfCombinations(n, k, combinations, resultList);
            combinations.RemoveAt(combinations.Count - 1);

            combinations.Add(-(combinations.Count + 1));
            GetListsOfCombinations(n, k, combinations, resultList);
            combinations.RemoveAt(combinations.Count - 1);
        }
    }
}
