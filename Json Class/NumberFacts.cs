using Xunit;

namespace JsonClasses
{
    public class NumberFacts
    {
        [Fact]
        public void StringIsNull()
        {
            var number = new Number();
            var text = new StringSpan(null);
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan(null, 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsEmpty()
        {
            var number = new Number();
            var text = new StringSpan("");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void CanBeZero()
        {
            var number = new Number();
            var text = new StringSpan("0");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("0", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void DoesNotContainLetters()
        {
            var number = new Number();
            var text = new StringSpan("a863");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("a863", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void CanHaveASingleDigit()
        {
            var number = new Number();
            var text = new StringSpan("1");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("1", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void CanHaveMultipleDigits()
        {
            var number = new Number();
            var text = new StringSpan("123");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("123", 3);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void NegativInteger()
        {
            var number = new Number();
            var text = new StringSpan("-165");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("-165", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void CantStartWithZero()
        {
            var number = new Number();
            var text = new StringSpan("07");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("07", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void CanBeFractional()
        {
            var number = new Number();
            var text = new StringSpan("12.34");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("12.34", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void TheFractionCanHaveLeadingZeros()
        {
            var number = new Number();
            var text1 = new StringSpan("0.00000001");
            var actualResult1 = number.Match(text1);
            var expectedResult1 = new StringSpan("0.00000001", 10);
            Assert.True(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));
            var text2 = new StringSpan("10.00000001");
            var actualResult2 = number.Match(text2);
            var expectedResult2 = new StringSpan("10.00000001", 11);
            Assert.True(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }

        [Fact]
        public void DoesNotEndWithADot()
        {
            var number = new Number();
            var text = new StringSpan("12.");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("12.", 2);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void DoesNotHaveTwoFractionParts()
        {
            var number = new Number();
            var text = new StringSpan("12.34.56");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("12.34.56", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void TheDecimalPartDoesNotAllowLetters()
        {
            var number = new Number();
            var text = new StringSpan("12.3x");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("12.3x", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void CanHaveAnExponent()
        {
            var number = new Number();
            var text = new StringSpan("12e3");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("12e3", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void TheExponentCanStartWithCapitalE()
        {
            var number = new Number();
            var text = new StringSpan("12E3");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("12E3", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void TheExponentCanHavePositive()
        {
            var number = new Number();
            var text = new StringSpan("12e+3");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("12e+3", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void TheExponentCanBeNegative()
        {
            var number = new Number();
            var text = new StringSpan("61e-9");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("61e-9", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void CanHaveFractionAndExponent()
        {
            var number = new Number();
            var text = new StringSpan("12.34E3");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("12.34E3", 7);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void TheExponentDoesNotAllowLetters()
        {
            var number = new Number();
            var text = new StringSpan("22e3x3");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("22e3x3", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void DoesNotHaveTwoExponents()
        {
            var number = new Number();
            var text = new StringSpan("22e323e33");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("22e323e33", 6);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void TheExponentIsAlwaysComplete()
        {
            var number = new Number();
            var text1 = new StringSpan("22e");
            var actualResult1 = number.Match(text1);
            var expectedResult1 = new StringSpan("22e", 2);
            Assert.True(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));
            
            var text2 = new StringSpan("22e+");
            var actualResult2 = number.Match(text2);
            var expectedResult2 = new StringSpan("22e+", 2);
            Assert.True(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));

            var text3 = new StringSpan("23E-");
            var actualResult3 = number.Match(text3);
            var expectedResult3 = new StringSpan("23E-", 2);
            Assert.True(actualResult3.Succes());
            Assert.True(expectedResult3.CheckIfEqualTo(actualResult3.RemainingText()));
        }

        [Fact]
        public void TheExponentIsAfterTheFraction()
        {
            var number = new Number();
            var text = new StringSpan("22e3.3");
            var actualResult = number.Match(text);
            var expectedResult = new StringSpan("22e3.3", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }
    }
}
