using Xunit;

namespace JsonClasses
{
    public class CharacterFacts
    {
        [Fact]
        public void StringStartsWithPattern()
        {
            Character c = new Character('0');
            Assert.True(c.Match("0sd1").Success());
        }

        [Fact]
        public void StringDoesntStartsWithPattern()
        {
            Character c = new Character('1');
            Assert.False(c.Match("dgahdg").Success());
        }

        [Fact]
        public void StringIsNullOrEmpty()
        {
            Character c = new('0');
            Assert.False(c.Match(null).Success());
            Assert.False(c.Match(string.Empty).Success());
        }
    }
}
