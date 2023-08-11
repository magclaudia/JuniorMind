using System;
using Xunit;

namespace JsonClasses
{
    public class MathFacts
    {
        [Fact]
        public void InvalidString_InputTextDoesNotContainNumbers()
        {
            var formula = new Math();
            var text = new StringSpan("asdf");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("asdf", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_ExpressionContainOneDigit()
        {
            var formula = new Math();
            var text = new StringSpan("1");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("1", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_SimpleMathematicalExpression()
        {
            var formula = new Math();
            var text = new StringSpan("1+2-1*6/1");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("1+2-1*6/1", 9);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_AdditionMathematicalExpression()
        {
            var formula = new Math();
            var text = new StringSpan("((1+2))");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("((1+2))", 7);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_InvertedBrackets()
        {
            var formula = new Math();
            var text = new StringSpan("(1+2(");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("(1+2(", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_DoubleBrackets()
        {
            var formula = new Math();
            var text = new StringSpan("((1+2-1))/(1+2)*2");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("((1+2-1))/(1+2)*2", 17);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_MultiplyVariablesOutcomeFromAddingAndDeacresing()
        {
            var formula = new Math();
            var text = new StringSpan("(1+2)*(3-4)");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("(1+2)*(3-4)", 11);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_MahematicalExpressionUsingDifferentTypesOfOperatorsAndMultiplesBrackets()
        {
            var formula = new Math();
            var text = new StringSpan("(1+2)*(3-4)/5^2+6%3*2^3");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("(1+2)*(3-4)/5^2+6%3*2^3", 23);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_MathematicalExpressionUsingDifferentTypesOfOperators()
        {
            var formula = new Math();
            var text = new StringSpan("(2*5)-3^2+9%3/3");
            var actualResult = formula.Match(text);
            var expectedREsult = new StringSpan("(2*5)-3^2+9%3/3", 15);
            Assert.True(actualResult.Succes());
            Assert.True(expectedREsult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_MathematicalExpresionWithMultipleOperatorsUsingFloatNumbers()
        {
            var formula = new Math();
            var text = new StringSpan("1.5*2^3*5/(2+3)-2^2");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("1.5*2^3*5/(2+3)-2^2", 19);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_MathematicalExpressionWithMultipleOperationsBetweenBrackets()
        {
            var formula = new Math();
            var text = new StringSpan("(1+2*3)/(4-5/6)*7");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("(1+2*3)/(4-5/6)*7", 17);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void InvalidString_MathematicalOperatorIsNotSeparatedByTwoVariablesNumbers()
        {
            var formula = new Math();
            var text = new StringSpan("(1+2)*(3-4)/5^+6%3*2^3");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("(1+2)*(3-4)/5^+6%3*2^3", 13);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void InvalidString_TwoOperatorsNextToEachOtherInMathematicalExpressionBetweenBrackets()
        {
            var formula = new Math();
            var text = new StringSpan("(2+4)*/(2-1)");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("(2+4)*/(2-1)", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void InvalidString_MathematicalExpressionEndsWithOperator()
        {
            var formula = new Math();
            var text = new StringSpan("1.5*2^3*5/(2+3)-");
            var actualResult = formula.Match(text);
            var expectedResult = new StringSpan("1.5*2^3*5/(2+3)-", 15);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void InvalidString_MathematicalOperatorIsNotSeparatedByTwoVariablesNumbersInsideBrackets()
        {
            var formula = new Math();
            var text = new StringSpan("(1+2*3)/(4-5/)*7");
            var actualResult = formula.Match(text);
            var expectedREsult = new StringSpan("(1+2*3)/(4-5/)*7", 7);
            Assert.True(actualResult.Succes());
            Assert.True(expectedREsult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void InvalidString_MathematicalExpressionDoesNotHasEndBracket()
        {
            var formula = new Math();
            var text = new StringSpan("(2+3");
            var actualResult = formula.Match(text);
            var expectedREsult = new StringSpan("(2+3", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedREsult.CheckIfEqualTo(actualResult.RemainingText()));
        }


        [Fact]
        public void InvalidString_SimpleMathOperation()
        {
            var formula = new Math();
            var text = new StringSpan("2++3");
            var actualResult = formula.Match(text);
            var expectedREsult = new StringSpan("2++3", 1);
            Assert.True(actualResult.Succes()); 
            Assert.True(expectedREsult.CheckIfEqualTo(actualResult.RemainingText()));
        }
    }
}
