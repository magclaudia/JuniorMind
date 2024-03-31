using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpressionsPolishForm
{
    public class ArithmeticExpressions
    {
        public static int CalculateReversePolishNotation(string expression)
        {
            var mathExpressionSplit = expression.Split(',').Select(x => x.Trim());
            var list = mathExpressionSplit.Aggregate(new List<int>(), (accumulator, element) =>
            {
                if (int.TryParse(element, out int result))
                {
                    accumulator.Add(result);
                }
                else
                {
                    var a = accumulator[^2];
                    var b = accumulator[^1];
                    accumulator.Add(Calculation(element, a, b));
                    accumulator.Remove(a);
                    accumulator.Remove(b);
                }

                return accumulator;
            });
            
            return list[^1];
        }

        private static int Calculation(string operator1, int previousElement, int nextElement)
        {
            switch(operator1)
            {
                case "+":
                    return previousElement + nextElement;
                case "-":
                    return previousElement - nextElement;
                case "*":
                    return previousElement * nextElement;
                case "/":
                    return previousElement / nextElement;
                default:
                    throw new ArgumentException($"Input element: '{operator1}' is not an valid operator");
            }
        }
    }
}
