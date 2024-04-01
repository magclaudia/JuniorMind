using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpressionsPolishForm
{
    public class ArithmeticExpressions
    {
        public static double CalculateReversePolishNotation(string expression)
        {
            var mathExpressionSplit = expression.Split(',').Select(x => x.Trim());
            var list = mathExpressionSplit.Aggregate(Enumerable.Empty<double>(), (accumulator, element) =>
            {
                if (double.TryParse(element, out double result))
                {
                    accumulator = accumulator.Append(result);
                }
                else
                {
                    var a = accumulator.SkipLast(1).Last();
                    var b = accumulator.Last();
                    accumulator = accumulator.Append(Calculation(element, a, b)).Where(index => index != b && index != a);
                }

                return accumulator;
            });
            
            return list.Last();
        }

        private static double Calculation(string operator1, double previousElement, double nextElement)
        {
            return operator1 switch
            {
                "+" => previousElement + nextElement,
                "-" => previousElement - nextElement,
                "*" => previousElement * nextElement,
                "/" => previousElement / nextElement,
                _ => throw new ArgumentException($"Input element: '{operator1}' is not an valid operator"),
            };
        }
    }
}
