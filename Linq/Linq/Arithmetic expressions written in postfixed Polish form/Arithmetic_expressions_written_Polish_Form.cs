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
            var list = new List<int>();
            foreach (var element in mathExpressionSplit)
            {
                if (int.TryParse(element, out int result))
                {
                    list.Add(result);
                }
                else
                {
                    var a = list[list.Count - 2];
                    var b = list[list.Count - 1];
                    list.Add(Calculation(element, a, b));
                    list.Remove(a);
                    list.Remove(b);
                }
            }
            
            return list[list.Count - 1];
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
                    throw new ArgumentException($"Input element: {0} is not an operator", operator1);
            }
        }
    }
}
