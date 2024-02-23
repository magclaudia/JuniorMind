using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqMethodsOnIntegers
{
    public class LinqMethodsOnIntegers
    {
        public static IEnumerable<IEnumerable<int>> CompareSublistIntegersSum(int[] integers, int k)
        {
            return GetSublists(integers).Where(sublist => sublist.Sum() <= k);
        }

        private static IEnumerable<IEnumerable<int>> GetSublists(int[] integers)
        {
            for (int i = 0; i < integers.Length; i++)
            {
                for (int j = i + 1; j <= integers.Length; j++)
                {
                    yield return integers[i..j];
                }
            }
        }
    }
}
