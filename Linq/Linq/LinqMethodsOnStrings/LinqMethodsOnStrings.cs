using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LinqMethods
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
            return GetSubsequences(text.ToCharArray()).Where(IsPalindrome).Select(sub => new string(sub.ToArray()));
        }

        public static IEnumerable<IEnumerable<T>> GetSubsequences<T>(T[] sequence)
        {
            IEnumerable<IEnumerable<T>> SubsequencesFromIndex(int i)
                => Enumerable.Range(i + 1, sequence.Length - i).Select(j => sequence[i..j]);
            return Enumerable.Range(0, sequence.Length).SelectMany(SubsequencesFromIndex);
        }

        private static bool IsPalindrome(IEnumerable<char> sub)
        {
            return sub.SequenceEqual(sub.Reverse());
        }
    }

    
}
