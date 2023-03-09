using System;

namespace JsonClasses
{
    public class Math : IPattern
    {
        private readonly IPattern pattern;

        public Math()
        {
            var space = new Optional(new Any(" "));
            var mathOperator = new Sequence(space, new Any("+-*/^%"), space);
            var operands = new Number();
            var simpleMathExpression = new OneOrMore(operands);
            var complexMathExpression = new OneOrMore(new Sequence(new Optional(
                new Character('(')), space, simpleMathExpression, space, new Optional(new Character(')'))));
            pattern = new Sequence(new List(new Choice(complexMathExpression, simpleMathExpression), mathOperator));
        }

        public IMatch Match(string text)
        {
            if (!CheckParentheses(text))
            {
                return new Match(false, text);
            }

            return pattern.Match(text);
        }

        private bool CheckParentheses(string text)
        {
            int bracketsCount = 0;
            foreach (char c in text)
            {
                if (c == '(')
                {
                    bracketsCount++;
                }
                else if (c == ')')
                {
                    if (bracketsCount == 0)
                    {
                        return false;
                    }
                    else
                    {
                        bracketsCount--;
                    }
                }
            }

            if (bracketsCount != 0)
            {
                return false;
            }

            return true;
        }
    }
}
