using System;
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
    }
}
