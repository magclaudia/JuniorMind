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
            GetCombinations(n, k, new List<int>(), resultList);
            return resultList;
        }

        private static void GetCombinations(int n, int k, List<int> combinations, List<List<int>> resultList)
        {
            if (combinations.Count == n)
            {
                if (combinations.Sum() == k)
                {
                    resultList.Add(new List<int>(combinations));
                }

                return;
            }

            GetCombinations(n, k, combinations.Concat(new int[] { combinations.Count + 1 }).ToList(), resultList);
            GetCombinations(n, k, combinations.Concat(new int[] { -(combinations.Count + 1)}).ToList(), resultList);
        }
    }
}
