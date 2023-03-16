using System;

namespace JsonClasses
{
    public class Math : IPattern
    {
        private readonly IPattern pattern;
        public Math()
        {
            var ws = new Many(new Any(" "));
            var mathOperator = new Sequence(ws, new Any("+-*/^%"), ws);
            var operand = new Choice(new Number());
            var expression = new List(operand, mathOperator);
            var complexMathExpression = new Sequence(new Character('('), ws, expression, ws, new Character(')'));
            operand.Add(complexMathExpression);
            pattern = expression;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
