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
            string text1 = "zaeiout1";
            (int, int) result1 = LinqMethodsOnStrings.VowelsAndConsonants(text1);
            (int, int) expectedResult1 = (2, 5); 
            Assert.Equal(expectedResult1, result1);

            string text2 = "AEDFSW@$>Q EASDQasr";
            (int, int) result2 = LinqMethodsOnStrings.VowelsAndConsonants(text2);
            (int, int) expectedResult2 = (10, 5);
            Assert.Equal(expectedResult2, result2);
        }

        [Fact]
        public void FirstNonRepeatingCharacter()
        {
            string text = "aakkb33lpb00";
            var result = LinqMethodsOnStrings.FirstNonRepeatingCharacter(text);
            var expectedResult = 'l';
            Assert.Equal(expectedResult, result);
        }
    }
}
