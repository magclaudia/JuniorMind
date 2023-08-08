using Xunit;

namespace JsonClasses
{
    public class ChoiceFacts
    {
        /*[Fact]
        public void ValidString_StringStartsWithRequiredChar()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.True(digit.Match("0").Succes());
            Assert.Equal("", digit.Match("0").RemainingText());
            Assert.True(hex.Match("a2314").Succes());
            Assert.Equal("2314", hex.Match("a2314").RemainingText());
            Assert.True(hex.Match("B2311").Succes());
            Assert.Equal("2311", hex.Match("B2311").RemainingText());
        }


        [Fact]
        public void StringInvalid_StringDoesNotStartWithRequiredChar()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.False(digit.Match("a1247").Succes());
            Assert.Equal("a1247", digit.Match("a1247").RemainingText());
            Assert.False(hex.Match("z1245").Succes());
            Assert.Equal("z1245", hex.Match("z1245").RemainingText());
        }

        [Fact]
        public void StringIsNull()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.False(digit.Match(null).Succes());
            Assert.Null(digit.Match(null).RemainingText());
            Assert.False(hex.Match(null).Succes());
            Assert.Null(hex.Match(null).RemainingText());
        }

        [Fact]
        public void StringIsEmpty()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            Assert.False(digit.Match("").Succes());
            Assert.Equal("", digit.Match("").RemainingText());
            Assert.False(hex.Match("").Succes());
            Assert.Equal("", hex.Match("").RemainingText());
        }*/
    }
}
