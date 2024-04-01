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
                  double.TryParse(element, out double result) 
                      ? accumulator.Append(result)
                        : accumulator.SkipLast(2).Append(Calculation(element, accumulator.TakeLast(2))));
            
            return list.Last();
        }

        private static double Calculation(string operators, IEnumerable<double> elements)
        {
            double previousElement = elements.First();
            double nextElement = elements.Last();
            return operators switch
            {
                "+" => previousElement + nextElement,
                "-" => previousElement - nextElement,
                "*" => previousElement * nextElement,
                "/" => previousElement / nextElement,
                _ => throw new ArgumentException($"Input element: '{operators}' is not an valid operator"),
            };
        }
    }
}
