using Xunit;

namespace JsonClasses
{
    public class RangeFacts
    {
        [Fact]
        public void StringStartsWithFirstCharFromRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("abc").Succes());
            Assert.Equal("bc", digit.Match("abc").RemainingText());
        }

        [Fact]
        public void StringStartsWithLastCharFromRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("fab").Succes());
            Assert.Equal("ab", digit.Match("fab").RemainingText());
        }

        [Fact]
        public void StringStartsWithACharThatIsInRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("bcd").Succes());
            Assert.Equal("cd", digit.Match("bcd").RemainingText());
        }

        [Fact]
        public void StringStartsWithACharThatIsNotInRange()
        {
            Range digit = new Range('a', 'f');
            Assert.False(digit.Match("1ab").Succes());
            Assert.Equal("1ab", digit.Match("1ab").RemainingText());
        }

        [Fact]
        public void StringIsNull()
        {
            Range digit = new Range('a', 'f');
            Assert.False(digit.Match(null).Succes());
            Assert.Null(digit.Match(null).RemainingText());
        }

        [Fact]
        public void StringIsEmpty()
        {
            Range digit = new Range('a', 'f');
            Assert.False(digit.Match(string.Empty).Succes());
            Assert.Equal("", digit.Match("").RemainingText());
        }
    }
}
