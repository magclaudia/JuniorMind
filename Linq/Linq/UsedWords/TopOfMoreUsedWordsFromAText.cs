using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsedWords
{
    public class TopOfMoreUsedWordsFromAText
    {
        public static IEnumerable<(string text, int numerOfElements)> MoreUsedWords(string text, int numberOfWords)
        {
            return text.Split().GroupBy(key => key).OrderByDescending(group => group.Count()).Take(numberOfWords).Select(x => (x.Key, x.Count()));
        }
    }
}
