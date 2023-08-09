using JsonClasses;
using System;

namespace JsonClasses
{
    public class String : IPattern
    {
        private readonly IPattern pattern;
        public String()
        {
            var quote = new Character('"');
            var hex = new Choice(new Range('1', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var escape = new Choice(new Any("\\\"/bfnrt"),
                new Sequence(new Character('u'), hex, hex, hex, hex));
            var character = new Choice(new Range('\u0020', '\u0021'),
                new Range('\u0023', '\u005B'), new Range('\u005D', '\uFFFF'),
                new Sequence(new Character('\\'), escape));
            var characters = new Many(character);
            pattern = new Sequence(quote, characters, quote);
        }

        public IMatch Match(StringSpan text)
        {
            return pattern.Match(text);
        }
    }
}
