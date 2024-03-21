using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UsedWords
{
    public class TopOfMoreUsedWordsFromATextFacts
    {
        [Fact]
        public void MoreUsedWords()
        {
            var text = "Ana are mere verzi pe care i le vinde lui Ovidiu care are mere galbene pe care i le vinde Anei pentru ca ea nu are .";
            int numberOfWords = text.Length;
            var result = TopOfMoreUsedWordsFromAText.MoreUsedWords(text, numberOfWords);
            
            List<(string, int)> expected = new() 
            {
                ("are", 3), ("care", 3), ("mere", 2), ("pe", 2), ("i", 2), ("le", 2), ("vinde", 2), ("Ana", 1), ("verzi", 1), ("lui", 1), 
                ("Ovidiu", 1), ("galbene", 1),  ("Anei", 1), ("pentru", 1), ("ca", 1), ("ea", 1), ("nu",1), (".", 1) 
            };
            
            Assert.Equal(expected, result);
        }
    }
}
