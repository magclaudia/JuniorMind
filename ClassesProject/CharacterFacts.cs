using Xunit;

namespace Classes
{
    public class CharacterFacts
    {
        [Fact]
        public void StringStartsWithPattern()
        {
            Character c = new Character('a');
            Assert.True(c.Match("asd1"));
        }

        [Fact]
        public void StringDoesntStartsWithPattern()
        {
            Character c = new Character('1');
            Assert.False(c.Match("dgahdg"));
        }

        [Fact]
        public void StringIsNullOrEmpty()
        {
            Character c = new('a');
            Assert.False(c.Match(null));
            Assert.False(c.Match(string.Empty));
        }
    }
}
