using Xunit;

namespace JsonClasses
{
    public class ChoiceFacts
    {
        [Fact]  
        public void Valid() 
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f') , new Range('A', 'F'));
            Assert.True(digit.Match("0").Succes());
            Assert.True(hex.Match("a231").Succes());
            Assert.True(hex.Match("B231").Succes());
        }

        [Fact]
        public void NotValid()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f') , new Range('A', 'F'));
            Assert.False(digit.Match("a").Succes());
            Assert.False(hex.Match("z").Succes());
        }

        [Fact]
        public void Negativ()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            Assert.True(digit.Match("-124").Succes());
        }
    }
}
