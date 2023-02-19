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
        }

        [Fact]
        public void StringStartsWithLastCharFromRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("fab").Succes());
        }
            
        [Fact]  
        public void StringStartsWithACharThatIsInRange()
        {
            Range digit = new Range('a', 'f');
            Assert.True(digit.Match("bcd").Succes());
        }

        [Fact]
        public void StringStartsWithACharThatIsNotInRange()
        {
            Range digit = new Range('a', 'f');
            Assert.False(digit.Match("1ab").Succes());
        }

        [Fact]
        public void StringIsNullOrEmpty()
        {
            Range digit = new Range('a', 'f');
            Character pattern = new('a');
            Assert.False(digit.Match(null).Succes());
            Assert.False(digit.Match(string.Empty).Succes());
            Assert.False(pattern.Match(null).Succes());
            Assert.False(pattern.Match(string.Empty).Succes());
        }
    }
}
