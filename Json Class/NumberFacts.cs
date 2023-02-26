using Xunit;

namespace JsonClasses
{
    public class NumberFacts
    {
        [Fact]
        public void StringIsNull()
        {
            var number = new Number();
            Assert.False(number.Match(null).Succes());
            Assert.Null(number.Match(null).RemainingText());
        }

        [Fact]
        public void StringIsEmpty()
        {
            var number = new Number();
            Assert.False(number.Match("").Succes());
            Assert.Equal("", number.Match("").RemainingText());
        }

        [Fact]
        public void CanBeZero()
        {
            var number = new Number();
            Assert.True(number.Match("0").Succes());
            Assert.Equal("", number.Match("0").RemainingText());
        }

        [Fact]
        public void DoesNotContainLetters()
        {
            var number = new Number();
            Assert.False(number.Match("a863").Succes());
            Assert.Equal("a863", number.Match("a863").RemainingText());
        }

        [Fact]
        public void CanHaveASingleDigit()
        {
            var number = new Number();
            Assert.True(number.Match("1").Succes());
            Assert.Equal("", number.Match("1").RemainingText());
        }

        [Fact]
        public void CanHaveMultipleDigits()
        {
            var number = new Number();
            Assert.True(number.Match("123").Succes());
            Assert.Equal("", number.Match("123").RemainingText());
        }

        [Fact]
        public void NegativInteger()
        {
            var number = new Number();
            Assert.True(number.Match("-165").Succes());
            Assert.Equal("", number.Match("-165").RemainingText());
        }

        [Fact]
        public void CantStartWithZero()
        {
            var number = new Number();
            Assert.True(number.Match("07").Succes());
            Assert.Equal("7", number.Match("07").RemainingText());
        }

        [Fact]
        public void CanBeFractional()
        {
            var number = new Number();
            Assert.True(number.Match("12.34").Succes());
            Assert.Equal("", number.Match("12.34").RemainingText());
        }

        [Fact]
        public void TheFractionCanHaveLeadingZeros()
        {
            var number = new Number();
            Assert.True(number.Match("0.00000001").Succes());
            Assert.Equal("", number.Match("0.00000001").RemainingText());
            Assert.True(number.Match("10.00000001").Succes());
            Assert.Equal("", number.Match("10.00000001").RemainingText());
        }

        [Fact]
        public void DoesNotEndWithADot()
        {
            var number = new Number();
            Assert.True(number.Match("12.").Succes());
            Assert.Equal(".", number.Match("12.").RemainingText());
        }

        [Fact]
        public void DoesNotHaveTwoFractionParts()
        {
            var number = new Number();
            Assert.True(number.Match("12.34.56").Succes());
            Assert.Equal(".56", number.Match("12.34.56").RemainingText());
        }

        [Fact]
        public void TheDecimalPartDoesNotAllowLetters()
        {
            var number = new Number();
            Assert.True(number.Match("12.3x").Succes());
            Assert.Equal("x", number.Match("12.3x").RemainingText());
        }

        [Fact]
        public void CanHaveAnExponent()
        {
            var number = new Number();
            Assert.True(number.Match("12e3").Succes());
            Assert.Equal("", number.Match("12e3").RemainingText());
        }

        [Fact]
        public void TheExponentCanStartWithCapitalE()
        {
            var number = new Number();
            Assert.True(number.Match("12E3").Succes());
            Assert.Equal("", number.Match("12E3").RemainingText());
        }

        [Fact]
        public void TheExponentCanHavePositive()
        {
            var number = new Number();
            Assert.True(number.Match("12e+3").Succes());
            Assert.Equal("", number.Match("12e+3").RemainingText());
        }

        [Fact]
        public void TheExponentCanBeNegative()
        {
            var number = new Number();
            Assert.True(number.Match("61e-9").Succes());
            Assert.Equal("", number.Match("61e-9").RemainingText());
        }

        [Fact]
        public void CanHaveFractionAndExponent()
        {
            var number = new Number();
            Assert.True(number.Match("12.34E3").Succes());
            Assert.Equal("", number.Match("12.34E3").RemainingText());
        }

        [Fact]
        public void TheExponentDoesNotAllowLetters()
        {
            var number = new Number();
            Assert.True(number.Match("22e3x3").Succes());
            Assert.Equal("x3", number.Match("22e3x3").RemainingText());
        }

        [Fact]
        public void DoesNotHaveTwoExponents()
        {
            var number = new Number();
            Assert.True(number.Match("22e323e33").Succes());
            Assert.Equal("e33", number.Match("22e323e33").RemainingText());
        }

        [Fact]
        public void TheExponentIsAlwaysComplete()
        {
            var number = new Number();
            Assert.True(number.Match("22e").Succes());
            Assert.Equal("e", number.Match("22e").RemainingText());
            Assert.True(number.Match("22e+").Succes());
            Assert.Equal("e+", number.Match("22e+").RemainingText());
            Assert.True(number.Match("23E-").Succes());
            Assert.Equal("E-", number.Match("23E-").RemainingText());
        }

        [Fact]
        public void TheExponentIsAfterTheFraction()
        {
            var number = new Number();
            Assert.True(number.Match("22e3.3").Succes());
            Assert.Equal(".3", number.Match("22e3.3").RemainingText());
        }
    }
}
