using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqLinqMethodsOnStrings
{
    public class LinqMethodsOnStringsFacts
    {
        [Fact]
        public void Identify_Vowels_And_Consonants()
        {
            string text = "zaeiout1";
            (int, int) result = LinqMethodsOnStrings.VowelsAndConsonants(text);
            (int, int) expectedResult = (2, 5); 
            Assert.Equal(expectedResult, result);
        }
    }
}
