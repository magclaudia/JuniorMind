
using Xunit;
using static Json.JsonNumber;

namespace Json.Facts
{
    public class JsonNumberFacts
    {
        [Fact]
        public void CanBeZero()
        {
            Assert.True(IsJsonNumber("0"));
        }

        [Fact]
        public void DoesNotContainLetters()
        {
            Assert.False(IsJsonNumber("a"));
        }

        [Fact]
        public void CanHaveASingleDigit()
        {
            Assert.True(IsJsonNumber("7"));
        }

        [Fact]
        public void CanHaveMultipleDigits()
        {
            Assert.True(IsJsonNumber("70"));
        }

        [Fact]
        public void IsNotNull()
        {
            Assert.False(IsJsonNumber(null));
        }

        [Fact]
        public void IsNotAnEmptyString()
        {
            Assert.False(IsJsonNumber(string.Empty));
        }

        [Fact]
        public void DoesNotStartWithZero()
        {
            Assert.False(IsJsonNumber("07"));
        }

        [Fact]
        public void CanBeNegative()
        {
            Assert.True(IsJsonNumber("-26"));
        }

        [Fact]
        public void CanBeMinusZero()
        {
            Assert.True(IsJsonNumber("-0"));
        }
    }
}
