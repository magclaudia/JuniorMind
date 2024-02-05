using Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinqLinqMethodsOnStrings
{
    public class LinqMethodsOnStrings
    {
        public static (int, int) VowelsAndConsonants(string text)
        {
            char[] isVowels = { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };
            int countVowels = text.Count(c => char.IsLetter(c) && isVowels.Contains(c));
            int countConsonants = text.Count(c => char.IsLetter(c) && !isVowels.Contains(c));
            return (countConsonants, countVowels);
        }
    }
}
