using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqMethodsOnIntegers
{
    public class LinqMethodsOnIntegersFacts
    {
        [Fact]
        public void CompareSubstringsSum()
        {
            int[] integers = { 1, 2, 3, 4, 5 };
            int k = 8;
            var result = LinqMethodsOnIntegers.CompareSublistIntegersSum(integers, k);
            var expected = new List<List<int>> 
            { 
                new List<int> { 1 }, 
                new List<int> { 1, 2 },
                new List<int> { 1, 2, 3 },
                new List<int> { 2 },
                new List<int> { 2, 3 },
                new List<int> { 3 },
                new List<int> { 3, 4 },
                new List<int> { 4 },
                new List<int> { 5 } 
            };

            Assert.Equal(expected, result);
        }
    }
}
