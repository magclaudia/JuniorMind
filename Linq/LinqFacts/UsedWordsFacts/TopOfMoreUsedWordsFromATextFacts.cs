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
            var result = TopOfMoreUsedWordsFromAText.MoreUsedWords(text);
            
            var expected = new List<string> { "are", "care", "mere", "pe", "i", "le", "vinde", "Ana", "verzi", "lui", 
                "Ovidiu", "galbene", "Anei", "pentru", "ca", "ea", "nu", "."};
            
            Assert.Equal(expected, result);
        }
    }
}
