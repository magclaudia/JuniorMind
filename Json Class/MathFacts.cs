using Xunit;

namespace JsonClasses
{
    public class MathFacts
    {
        [Fact]
        public void ValidString_AdditionSimpleFormula()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 )").Succes());
            Assert.Equal("", formula.Match("( 1 + 2 )").RemainingText());
        }

        [Fact]
        public void ValidString_MultiplyingTheTwoNumbersResultingFromTheAddition()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 ) * ( 3 - 4 )").Succes());
            Assert.Equal("", formula.Match("( 1 + 2 ) * ( 3 - 4 )").RemainingText());
        }

        [Fact]
        public void ValidString_PerformingComplexOperationsWithMultipleOperations()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 ) * ( 3 - 4 ) / 5 ^ 2 + 6 % 3 * 2 ^ 3").Succes());
            Assert.Equal("", formula.Match("( 1 + 2 ) * ( 3 - 4 ) / 5 ^ 2 + 6 % 3 * 2 ^ 3").RemainingText());
        }

        [Fact]
        public void ValidString_ComplexFormula()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 2 * 5 ) - 3 ^ 2 + 9 % 3 / 3").Succes());
            Assert.Equal("", formula.Match("( 2 * 5 ) - 3 ^ 2 + 9 % 3 / 3").RemainingText());
        }

        [Fact]
        public void ValidString_ComplexFormulaUsingFloatDigitAndMultipleOperations()
        {
            var formula = new Math();
            Assert.True(formula.Match("1.5 * 2 ^ 3 * 5 / ( 2 + 3 ) - 2 ^ 2").Succes());
            Assert.Equal("", formula.Match("1.5 * 2 ^ 3 * 5 / ( 2 + 3 ) - 2 ^ 2").RemainingText());
        }

        [Fact]
        public void ValidString_MultipleOperationsBetweenBrackets()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 * 3 ) / ( 4 - 5 / 6 ) * 7").Succes());
            Assert.Equal("", formula.Match("( 1 + 2 * 3 ) / ( 4 - 5 / 6 ) * 7").RemainingText());
        }

        [Fact]
        public void InvalidString_MathematicalOperationsAreNotSeparatedByNumbers()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 ) * ( 3 - 4 ) / 5 ^ + 6 % 3 * 2 ^ 3").Succes());
            Assert.Equal(" ^ + 6 % 3 * 2 ^ 3", formula.Match("( 1 + 2 ) * ( 3 - 4 ) / 5 ^ + 6 % 3 * 2 ^ 3").RemainingText());
        }

        [Fact]
        public void InvalidString_TwoSignNextToEacHOtherBetweenBracketsOperations()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 2 + 4 ) * / ( 2 - 1 )").Succes());
            Assert.Equal(" * / ( 2 - 1 )", formula.Match("( 2 + 4 ) * / ( 2 - 1 )").RemainingText());

        }

        [Fact]
        public void InvalidString_MathematicalOperationsEndsWithSign()
        {
            var formula = new Math();
            Assert.True(formula.Match("1.5 * 2 ^ 3 * 5 / ( 2 + 3 ) -").Succes());
            Assert.Equal(" -", formula.Match("1.5 * 2 ^ 3 * 5 / ( 2 + 3 ) -").RemainingText());

        }

        [Fact]
        public void InvalidString_MathematicalOperationsAreNotSeparatedByNumbersInsideBrackets()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 1 + 2 * 3 ) / ( 4 - 5 / ) * 7").Succes());
            Assert.Equal(" / ( 4 - 5 / ) * 7", formula.Match("( 1 + 2 * 3 ) / ( 4 - 5 / ) * 7").RemainingText());

        }

        [Fact]
        public void InvalidString_MathematicalOperationHasNotEndBracket()
        {
            var formula = new Math();
            Assert.True(formula.Match("( 2 + 3").Succes());
            Assert.Equal("( 2 + 3", formula.Match("( 2 + 3").RemainingText());
        }
    }
}
