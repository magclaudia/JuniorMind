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
            return  text.Where(char.IsLetter).Aggregate((consonants: 0, vowels: 0), (count, c) =>
            isVowels.Contains(c) ? (count.consonants, count.vowels + 1) : (count.consonants + 1, count.vowels));
        }
    }
}
