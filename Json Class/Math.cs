using System;

namespace JsonClasses
{
    public class Math : IPattern
    {
        private readonly IPattern pattern;
        public Math()
        {
            var space = new Any(" ");
            var expressionOperator = new Sequence(space, new Any("+-*/^%"), space);
            var brackets = new Optional(new Any("()"));
            var operands = new Number();
            var simpleMathematicalExpression = new OneOrMore(new Sequence(new Optional(expressionOperator), operands));
            var complexMathematicalExpression = new OneOrMore(new Sequence(brackets, space, simpleMathematicalExpression, space, brackets));
            pattern = new Sequence(new List(new Choice(complexMathematicalExpression, simpleMathematicalExpression), expressionOperator));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}