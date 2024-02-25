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
            IEnumerable<IEnumerable<int>> SublistsIndex(int i) =>
                Enumerable.Range(i + 1, integers.Length - i).Select(j => integers[i..j]);
            return Enumerable.Range(0, integers.Length).SelectMany(SublistsIndex);
        }
    }
}
