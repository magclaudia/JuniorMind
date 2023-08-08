using JsonClasses;
using System;

namespace JsonClasses
{
    public class Number : IPattern
    {
        private readonly IPattern pattern;

        public Number()
        {
            var onenine = new Range('1', '9');
            var digit = new Choice(new Character('0'), onenine);
            var digits = new OneOrMore(digit);
            var integer = new Sequence(new Optional(new Character('-')),
                new Choice(new Sequence(onenine, digits), digit));
            var fraction = new Optional(new Sequence(new Character('.'), digits));
            var exponent = new Optional(new Sequence(new Any("eE"),
                new Optional(new Any("+-")), digits));
            pattern = new Sequence(integer, fraction, exponent);
        }

        public IMatch Match(StringWrapper text)
        {
            return pattern.Match(text);
        }
    }
}
