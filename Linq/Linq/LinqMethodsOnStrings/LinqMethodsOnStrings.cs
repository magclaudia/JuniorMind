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
            int vowels = 0;
            int consonants = 0;
            return text.Aggregate((consonants, vowels), (count, c) => char.IsLetter(c) ? 
            (isVowels.Contains(c) ? (count.consonants, count.vowels + 1) : (count.consonants + 1, count.vowels)) : count);
        }
    }
}
