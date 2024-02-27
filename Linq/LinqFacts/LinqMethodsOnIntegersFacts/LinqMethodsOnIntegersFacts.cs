using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqMethods
{
    public class LinqMethodsOnIntegersFacts
    {
        [Fact]
        public void CompareSubstringsSumFirstTry()
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

        [Fact]
        public void CompareSubstringsSumSecondTry()
        {
            int[] integers = { 1, 2, 3, 4, 5 };
            int k = 12;
            var result = LinqMethodsOnIntegers.CompareSublistIntegersSum(integers, k);
            var expected = new List<List<int>>
            {
                new List<int> { 1 },
                new List<int> { 1, 2 },
                new List<int> { 1, 2, 3 },
                new List<int> { 1, 2, 3, 4 },
                new List<int> { 2 },
                new List<int> { 2, 3 },
                new List<int> { 2, 3, 4 },
                new List<int> { 3 },
                new List<int> { 3, 4 },
                new List<int> { 3, 4, 5 },
                new List<int> { 4 },
                new List<int> { 4, 5 },
                new List<int> { 5 }
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CompareSubstringsSumThirdTry()
        {
            int[] integers = { 1, 2, 3, 4, 5 };
            int k = 5;
            var result = LinqMethodsOnIntegers.CompareSublistIntegersSum(integers, k);
            var expected = new List<List<int>>
            {
                new List<int> { 1 },
                new List<int> { 1, 2 },
                new List<int> { 2 },
                new List<int> { 2, 3 },
                new List<int> { 3 },
                new List<int> { 4 },
                new List<int> { 5 }
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CompareSubstringsSumFourTry()
        {
            int[] integers = { 1, 2, 3, 4, 5 };
            int k = 2;
            var result = LinqMethodsOnIntegers.CompareSublistIntegersSum(integers, k);
            var expected = new List<List<int>>
            {
                new List<int> { 1 },
                new List<int> { 2 },
            };

            Assert.Equal(expected, result);
        }
    }
}
