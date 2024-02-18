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

        [Fact]
        public void StringToInt_CorrectlyConvertStringToInteger()
        {
            string text = "1214";
            var result = LinqMethodsOnStrings.StringToInt(text);
            var expectedResult = 1214;
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void StringToInt_ItsNotDigit() 
        {
            string text1 = "232d3";
            var expectedResult1 = Assert.Throws<ArgumentException>(() => LinqMethodsOnStrings.StringToInt(text1));
            Assert.Equal("Input text is not a digit", expectedResult1.Message);
        }

        [Fact]
        public void StingToInt_ItsNegative() 
        {
            string text = "-1245";
            var result = LinqMethodsOnStrings.StringToInt(text);
            var expectedResult = -1245;
            Assert.Equal(expectedResult, result);   
        }

        [Fact]
        public void StingToInt_LargeNumber()
        {
            string text = "214748364";
            var result = LinqMethodsOnStrings.StringToInt(text);
            var expectedResult = 214748364;
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void StingToInt_InputTextEmptyString()
        {
            string text = string.Empty;
            Assert.Throws<ArgumentNullException>(() => LinqMethodsOnStrings.StringToInt(text));
        }

        [Fact]
        public void StingToInt_InputTextIsNull()
        {
            string text = null;
            Assert.Throws<ArgumentNullException>(() => LinqMethodsOnStrings.StringToInt(text));
        }

        [Fact]
        public void StingToInt_InputTextIsOneDigit() 
        {
            string text = "1";
            var result = LinqMethodsOnStrings.StringToInt(text);
            var expected = 1;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void StingToInt_InputTextStartWithZero()
        {
            string text = "0123";
            var result = LinqMethodsOnStrings.StringToInt(text);
            var expected = 123;
            Assert.Equal(expected, result);
        }


        [Fact]
        public void MaxNumberOfOccurencesOfACharInString()
        {
            string text = "sadaada";
            var result = LinqMethodsOnStrings.CharacterWithMaximumNumberOfOccurrences(text);
            var expectedResult = 'a';
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void MaxNumberOfOccurencesOfACharInString_EqualMaxCount()
        {
            string text = "dlloppfd";
            var result = LinqMethodsOnStrings.CharacterWithMaximumNumberOfOccurrences(text);
            var expected = 'd';
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Palindrome()
        {
            string text = "aabaac";
            var result = LinqMethodsOnStrings.Palindorme(text);
            var expected = new List<string> { "a", "aa", "aabaa", "a", "aba", "b", "a", "aa", "a", "c" };
            Assert.Equal(expected, result);
        }
    }
}
