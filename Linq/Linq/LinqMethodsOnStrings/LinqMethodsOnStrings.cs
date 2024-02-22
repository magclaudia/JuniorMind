using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LinqLinqMethodsOnStrings
{
    public class LinqMethodsOnStrings
    {
        public static (int consonants, int vowels) VowelsAndConsonants(string text)
        {
            string isVowels = "aeiouAEIOU";
            return text.Where(char.IsLetter).Aggregate((consonants: 0, vowels: 0), (count, c) =>
            isVowels.Contains(c) ? (count.consonants, count.vowels + 1) : (count.consonants + 1, count.vowels));
        }

        public static char FirstNonRepeatingCharacter(string text)
        {
            return text.GroupBy(element => element).First(element => element.Count() == 1).Key;
        }

        public static int StringToInt(string text)
        {
            int integerSign = 1;
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }

            if (text.StartsWith('-'))
            {
                integerSign = -1;
                text = text.Substring(1);
            }
            else if (!text.All(char.IsDigit))
            {
                throw new ArgumentException("Input text is not a digit");
            }
            
            return text.Aggregate(0, (integer, character) => integer * 10  + (character - '0')) * integerSign;
        }

        public static char CharacterWithMaximumNumberOfOccurrences(string text)
        {
            return text.GroupBy(element => element).MaxBy(element => element.Count())!.Key;
        }

        public static IEnumerable<string> Palindrome(string text)
        {
            return text.SelectMany((character, startIndex) => GetSubstrings(text, startIndex)).Where(IsPalindrome);
        }

        private static IEnumerable<string> GetSubstrings(string text, int startIndex)
        {
            int maxLength = text.Length - startIndex;
            return text.Substring(startIndex, maxLength).Select((character, length) => text.Substring(startIndex, length + 1));
        }

        private static bool IsPalindrome(string substring)
        {
            return substring.SequenceEqual(substring.Reverse());
        }
    }
}
