using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqLinqMethodsOnStrings
{
    public class LinqMethodsOnStrings
    {
        public static (int, int) VowelsAndConsonants(string text)
        {
            char[] isVowels = { 'a', 'e', 'i', 'o', 'u'};
            int countVowels = 0;
            int countConsonants = 0;
            foreach (char c in text) 
            {
                if (char.IsLetter(c) && isVowels.Contains(c))
                {
                    countVowels++;
                }
                else if (char.IsLetter(c))
                {
                    countConsonants++;
                }
            }

            return (countConsonants, countVowels);
        }
    }
}
