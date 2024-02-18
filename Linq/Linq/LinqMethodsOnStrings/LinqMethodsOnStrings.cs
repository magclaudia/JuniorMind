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

        public static IEnumerable<string> Palindorme(string text)
        {
            List<string> list = new List<string>();
            string element = string.Empty;
            char[] verification;
            for (int i = 0; i < text.Length; i++) 
            {
                element += text[i];
                list.Add(element);
                for (int j = i + 1; j < text.Length; j++)
                {
                    element += text[j];
                    verification = element.ToCharArray();
                    Array.Reverse(verification);
                    if (element == new string(verification))
                    {
                        list.Add(element);
                    }
                }

                element = string.Empty;
            }

            return list;
        }
    }
}
