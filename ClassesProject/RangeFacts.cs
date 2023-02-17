using Xunit;

namespace Classes
{
    public class RangeFacts
    {
        [Fact]  
        public void StringStartsWithFirstCharFromRange()
        {
            Range c = new Range('a', 'f');
            Assert.True(c.Match("abc"));
        }

        [Fact]
        public void StringStartsWithLastCharFromRange()
        {
            Range c = new Range('a', 'f');
            Assert.True(c.Match("fab"));
        }

        [Fact]
        public void StringStartsWithACharThatIsInRange()
        {
            Range c = new Range('a', 'f');
            Assert.True(c.Match("bcd"));
        }

        [Fact]
        public void StringStartsWithACharThatIsNotInRange()
        {
            Range c = new Range('a', 'f');
            Assert.False(c.Match("1ab"));
        }

        [Fact]
        public void StringIsNullOrEmpty()
        {
            Range c = new Range('a', 'f');
            Character pattern = new('a');
            Assert.False(c.Match(null));
            Assert.False(c.Match(string.Empty));
            Assert.False(pattern.Match(null));
            Assert.False(pattern.Match(string.Empty));
        }
    }
}
