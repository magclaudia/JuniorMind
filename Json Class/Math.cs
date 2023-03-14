using System;

namespace JsonClasses
{
    public class Math : IPattern
    {
        private readonly IPattern pattern;
        public Math()
        {
            var space = new Any(" ");
            var mathOperator = new Sequence(space, new Any("+-*/^%"), space);
            var operand = new Number();
            var simpleMathExpression = new List(operand, mathOperator);
            var complexMathExpression = new Sequence(new OneOrMore(new Character('(')), space, simpleMathExpression, space, new OneOrMore(new Character(')')));
            pattern = new List(new Choice(complexMathExpression, simpleMathExpression), mathOperator);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}