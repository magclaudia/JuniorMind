using Xunit;

namespace Classes
{
    public class RangeFacts
    {
        [Fact]  
        public void StringStartsWithFirstCharFromRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("abc"));
        }

        [Fact]
        public void StringStartsWithLastCharFromRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("fab"));
        }

        [Fact]
        public void StringStartsWithACharThatIsInRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("bcd"));
        }

        [Fact]
        public void StringStartsWithACharThatIsNotInRange()
        {
            Range digit = new Range('a', 'f');
            Assert.False(digit.Match("1ab"));
        }

        [Fact]
        public void StringIsNullOrEmpty()
        {
            Range digit = new Range('a', 'f');
            Character pattern = new('a');
            Assert.False(digit.Match(null));
            Assert.False(digit.Match(string.Empty));
            Assert.False(pattern.Match(null));
            Assert.False(pattern.Match(string.Empty));
        }
    }
}
