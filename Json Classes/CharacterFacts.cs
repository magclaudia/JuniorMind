using Xunit;

namespace JsonClasses
{
    public class CharacterFacts
    {
        [Fact]
        public void StringStartsWithPattern()
        {
            Character c = new Character('0');
            Assert.True(c.Match("0sd1").Succes());
        }

        [Fact]
        public void StringDoesntStartsWithPattern()
        {
            Character c = new Character('1');
            Assert.False(c.Match("dgahdg").Succes());
        }

        [Fact]
        public void StringIsNullOrEmpty()
        {
            Character c = new('0');
            Assert.False(c.Match(null).Succes());
            Assert.False(c.Match(string.Empty).Succes());
        }
    }
}
