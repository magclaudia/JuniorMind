using System;

namespace JsonClasses
{
    public class Math : IPattern
    {
        private readonly IPattern pattern;
        public Math()
        {
            var space = new Any("' '");
            var sign = new Any("+-*/^%");
            var brackets = new Any("( )");
            var digit = new Number();
            var numbersInBrackets = new Sequence(brackets, space, digit,
                new OneOrMore(new Sequence(space, sign, space, digit)),
                space, brackets);

            var formulas = new Sequence(new Optional(new Sequence(space, sign, space)),
                new Choice(numbersInBrackets, digit));
            pattern = new Many(formulas);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}