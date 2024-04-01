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
        public void ArithmeticExpressionsPolishForm_CheckIfItWorksWithDuplicatesValues()
        {
            var mathExpression = "5, 6, 2, +, *, 40, 40, /, -";
            var result = ArithmeticExpressions.CalculateReversePolishNotation(mathExpression);
            var expected = 39;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ArithmeticExpressionsPolishForm1_InvalidOperator()
        {
            var mathExpression = "5, 6, 2, a, +, *, 12, 4, /, -";
            Assert.Throws<ArgumentException>(() => ArithmeticExpressions.CalculateReversePolishNotation(mathExpression));
        }

        [Fact]
        public void ArithmeticExpressionsPolishForm_CheckIfItWorksForDouble()
        {
            var mathExpression = "5.01, 6, 2.24, +, *, 12.25, 4, /, -";
            var result = ArithmeticExpressions.CalculateReversePolishNotation(mathExpression);
            var expected = 38.2199;
            Assert.Equal(expected, result);
        }
    }
}
