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
            Assert.False(formula.Match("asdf").Succes());
            Assert.Equal("asdf", formula.Match("asdf").RemainingText());
        }

        [Fact]
        public void ValidString_ExpressionContainOneDigit()
        {
            var formula = new Math();
            Assert.True(formula.Match("1").Succes());
            Assert.Equal("", formula.Match("1").RemainingText());
        }

        [Fact]
        public void ValidString_SimpleMathematicalExpression()
        {
            var formula = new Math();
            Assert.True(formula.Match("1 + 2 - 1 * 6 / 1").Succes());
            Assert.Equal("", formula.Match("1 + 2 - 1 * 6 / 1").RemainingText());
        }

        [Fact]
        public void ValidString_AdditionMathematicalExpression()
        {
            var formula = new Math();
            Assert.True(formula.Match("(( 1 + 2 ))").Succes());
            Assert.Equal("", formula.Match("(( 1 + 2 ))").RemainingText());
        }

        [Fact]
        public void ValidString_InvertedBrackets()
        {
            var formula = new Math();
            Assert.False(formula.Match("( 1 + 2 (").Succes());
            Assert.Equal("( 1 + 2 (", formula.Match("( 1 + 2 (").RemainingText());
        }

        [Fact]
        public void ValidString_DoubleBrackets()
        {
            var formula = new Math();
            Assert.True(formula.Match("(( 1 + 2 - 1 )) / ( 1 + 2 ) * 2").Succes());
            Assert.Equal("", formula.Match("(( 1 + 2 - 1 )) / ( 1 + 2 ) * 2").RemainingText());
        }

        [Fact]
        public void ValidString_MultiplyVariablesOutcomeFromAddingAndDeacresing()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 ) * ( 3 - 4 )").Succes());
            Assert.Equal("", formula.Match("( 1 + 2 ) * ( 3 - 4 )").RemainingText());
        }

        [Fact]
        public void ValidString_MahematicalExpressionUsingDifferentTypesOfOperatorsAndMultiplesBrackets()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 ) * ( 3 - 4 ) / 5 ^ 2 + 6 % 3 * 2 ^ 3").Succes());
            Assert.Equal("", formula.Match("( 1 + 2 ) * ( 3 - 4 ) / 5 ^ 2 + 6 % 3 * 2 ^ 3").RemainingText());
        }

        [Fact]
        public void ValidString_MathematicalExpressionUsingDifferentTypesOfOperators()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 2 * 5 ) - 3 ^ 2 + 9 % 3 / 3").Succes());
            Assert.Equal("", formula.Match("( 2 * 5 ) - 3 ^ 2 + 9 % 3 / 3").RemainingText());
        }

        [Fact]
        public void ValidString_MathematicalExpresionWithMultipleOperatorsUsingFloatNumbers()
        {
            var formula = new Math();
            Assert.True(formula.Match("1.5 * 2 ^ 3 * 5 / ( 2 + 3 ) - 2 ^ 2").Succes());
            Assert.Equal("", formula.Match("1.5 * 2 ^ 3 * 5 / ( 2 + 3 ) - 2 ^ 2").RemainingText());
        }

        [Fact]
        public void ValidString_MathematicalExpressionWithMultipleOperationsBetweenBrackets()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 * 3 ) / ( 4 - 5 / 6 ) * 7").Succes());
            Assert.Equal("", formula.Match("( 1 + 2 * 3 ) / ( 4 - 5 / 6 ) * 7").RemainingText());
        }

        [Fact]
        public void InvalidString_MathematicalOperatorIsNotSeparatedByTwoVariablesNumbers()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 ) * ( 3 - 4 ) / 5 ^ + 6 % 3 * 2 ^ 3").Succes());
            Assert.Equal(" ^ + 6 % 3 * 2 ^ 3", formula.Match("( 1 + 2 ) * ( 3 - 4 ) / 5 ^ + 6 % 3 * 2 ^ 3").RemainingText());
        }

        [Fact]
        public void InvalidString_TwoOperatorsNextToEachOtherInMathematicalExpressionBetweenBrackets()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 2 + 4 ) * / ( 2 - 1 )").Succes());
            Assert.Equal(" * / ( 2 - 1 )", formula.Match("( 2 + 4 ) * / ( 2 - 1 )").RemainingText());
        }

        [Fact]
        public void InvalidString_MathematicalExpressionEndsWithOperator()
        {
            var formula = new Math();
            Assert.True(formula.Match("1.5 * 2 ^ 3 * 5 / ( 2 + 3 ) -").Succes());
            Assert.Equal(" -", formula.Match("1.5 * 2 ^ 3 * 5 / ( 2 + 3 ) -").RemainingText());
        }

        [Fact]
        public void InvalidString_MathematicalOperatorIsNotSeparatedByTwoVariablesNumbersInsideBrackets()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 * 3 ) / ( 4 - 5 / ) * 7").Succes());
            Assert.Equal(" / ( 4 - 5 / ) * 7", formula.Match("( 1 + 2 * 3 ) / ( 4 - 5 / ) * 7").RemainingText());
        }

        [Fact]
        public void InvalidString_MathematicalExpressionDoesNotHasEndBracket()
        {
            var formula = new Math();
            Assert.False(formula.Match("( 2 + 3").Succes());
            Assert.Equal("( 2 + 3", formula.Match("( 2 + 3").RemainingText());
        }


        [Fact]
        public void InvalidString_SimpleMathOperation()
        {
            var formula = new Math();
            Assert.True(formula.Match("2 + + 3").Succes());
            Assert.Equal(" + + 3", formula.Match("2 + + 3").RemainingText());
        }
    }
}
