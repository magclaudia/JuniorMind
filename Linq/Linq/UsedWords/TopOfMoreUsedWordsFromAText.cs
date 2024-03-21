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
            return text.Split().GroupBy(key => key).Select(x => (Word: x.Key, Count: x.Count())).OrderByDescending(group => group.Count);
        }
    }
}
