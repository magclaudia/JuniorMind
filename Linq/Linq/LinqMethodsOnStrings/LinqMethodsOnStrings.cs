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
            int posibleNevative = 1;
            if (!text.All(char.IsDigit) && !text.StartsWith('-'))
            {
                throw new ArgumentException("Input text is not a digit");
            }
            else if (text.StartsWith('-'))
            {
                posibleNevative = -1;
                text = text.Substring(1);
            }

            return text.Aggregate(0, (integer, character) => integer = integer * 10  + (int)char.GetNumericValue(character)) * posibleNevative;
        }
    }
}
