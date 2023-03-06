using System;

namespace JsonClasses
{
    public class Math : IPattern
    {
        private readonly IPattern pattern;
        public Math()
        {
            var space = new Any("' '");
            var sign = new Duplicates("+-*/^%");
            var digit = new Range('1', '9');
            var brackets = new Any("( )");
            var floatNumber = new Sequence(new Character('.'), digit);
            var simpleOperation = new Sequence(digit, new Optional(floatNumber));
            var numbersInBrackets = new Sequence(brackets, space, digit,
                new Optional(floatNumber), new OneOrMore(new Sequence(space, sign, space, digit,
                    new Optional(floatNumber))), space, brackets);
            var formulas = new Sequence(new Optional(new Sequence(space, sign, space)),
                new Choice(numbersInBrackets, simpleOperation), new Optional(new Sequence(space, sign, space, digit)));
            pattern = new Many(formulas);
        }


        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
