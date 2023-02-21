using Xunit;

namespace JsonClasses
{
    public class OneOrMoreFacts
    {
        [Fact]
        public void ValidString_TextIsEqualWithPrefix()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.True(a.Match("123").Succes());
            Assert.Equal("", a.Match("123").RemainingText());
        }

        [Fact]
        public void ValidString_TextContainPrefix()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.True(a.Match("1a").Succes());
            Assert.Equal("a", a.Match("1a").RemainingText());
        }

        [Fact]
        public void ValidString_TextDoesNotContainPrefix()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.False(a.Match("bc").Succes());
            Assert.Equal("bc", a.Match("bc").RemainingText());
        }

        [Fact]
        public void StringIsEmpty()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.False(a.Match("").Succes());
            Assert.Equal("", a.Match("").RemainingText());
        }

        [Fact]
        public void StringIsNull()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.False(a.Match(null).Succes());
            Assert.Null(a.Match(null).RemainingText());
        }
    }
}
