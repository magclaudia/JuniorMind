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
            var operands = new Number();
            var simpleMathExpression = new List(operands, mathOperator);
            var complexMathExpression = new Sequence(new Character('('), space, simpleMathExpression, space, new Character(')'));
            pattern = new List(new Choice(complexMathExpression, simpleMathExpression), mathOperator);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
