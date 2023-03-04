using System;

namespace JsonClasses
{
    public class Math : IPattern
    {
        private readonly IPattern pattern;
        public Math()
        {
            var space = new Many(new Any("''"));
            var sign = new Many(new Any("+-*/^%"));
            var element = new Sequence(space, sign, space);
            var digit = new Sequence(space, new OneOrMore(new Range('1', '9')), space);
            var numbersInBrackets = new Sequence(new Character('('), digit, element, digit, new Character(')'));
            pattern = numbersInBrackets;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
