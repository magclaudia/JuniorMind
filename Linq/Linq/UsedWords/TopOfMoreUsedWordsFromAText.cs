using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsedWords
{
    public class TopOfMoreUsedWordsFromAText
    {
        public static IEnumerable<string> MoreUsedWords(string text)
        {
            return text.Split().GroupBy(key => key).OrderByDescending(group => group.Count()).Select(x => x.Key);
        }
    }
}
