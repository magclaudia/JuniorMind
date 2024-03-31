using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ArithmeticExpressionsPolishForm
{
    public class ArithmeticExpressionsFacts
    {
        [Fact]
        public void ArithmeticExpressionsPolishForm()
        {
            var mathExpression = "5, 6, 2, +, *, 12, 4, /, -";
            var result = ArithmeticExpressions.CalculateReversePolishNotation(mathExpression);
            var expected = 37;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ArithmeticExpressionsPolishForm1_InvalidOperator()
        {
            var mathExpression = "5, 6, 2, a, +, *, 12, 4, /, -";
            Assert.Throws<ArgumentException>(() => ArithmeticExpressions.CalculateReversePolishNotation(mathExpression));
        }
    }
}
