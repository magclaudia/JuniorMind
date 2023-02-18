using Xunit;

namespace JsonClasses
{
    public class RangeFacts
    {
        [Fact]  
        public void StringStartsWithFirstCharFromRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("abc").Success());
        }

        [Fact]
        public void StringStartsWithLastCharFromRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("fab").Success());
        }
            
        [Fact]  
        public void StringStartsWithACharThatIsInRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("bcd").Success());
        }

        [Fact]
        public void StringStartsWithACharThatIsNotInRange()
        {
            Range digit = new Range('a', 'f');
            Assert.False(digit.Match("1ab").Success());
        }

        [Fact]
        public void StringIsNullOrEmpty()
        {
            Range digit = new Range('a', 'f');
            Character pattern = new('a');
            Assert.False(digit.Match(null).Success());
            Assert.False(digit.Match(string.Empty).Success());
            Assert.False(pattern.Match(null).Success());
            Assert.False(pattern.Match(string.Empty).Success());
        }
    }
}
