using System;

namespace JsonClasses
{
    public class Math : IPattern
    {
        private readonly IPattern pattern;
        public Math()
        {
            var space = new Many(new Any(" "));
            var mathOperator = new Sequence(space, new Any("+-*/^%"), space);
            var operand = new Choice(new Number());
            var expression = new List(operand, mathOperator);
            var complexMathExpression = new Sequence(new Character('('), space, expression, space, new Character(')'));
            operand.Add(complexMathExpression);
            pattern = expression;
        }

        public IMatch Match(string text)
        {
            IMatch match = pattern.Match(text);
            if (match.RemainingText() == text)
            {
                return new Match(false, text);
            }

            return match;
        }
    }
}
