using Xunit;

namespace JsonClasses
{
    public class TextFacts
    {
        /*[Fact]
        public void ValidString_StringHasRequiredPrefix()
        {
            var prefix = new Text("true");
            Assert.True(prefix.Match("true").Succes());
            Assert.Equal("", prefix.Match("true").RemainingText());
        }

        [Fact]
        public void ValidString_StringHasRequiredPrefixAndRemainigText()
        {
            var prefix = new Text("true");
            Assert.True(prefix.Match("trueX").Succes());
            Assert.Equal("X", prefix.Match("trueX").RemainingText());
        }

        [Fact]
        public void InvalidString_StringHasRequiredPrefixAndRemainigText()
        {
            var prefix = new Text("true");
            Assert.False(prefix.Match("false").Succes());
            Assert.Equal("false", prefix.Match("false").RemainingText());
        }

        [Fact]
        public void StringIsEmptyString()
        {
            var prefix = new Text("true");
            Assert.False(prefix.Match("").Succes());
            Assert.Equal("", prefix.Match("").RemainingText());
        }

        [Fact]
        public void StringIsNull()
        {
            var prefix = new Text("true");
            Assert.False(prefix.Match(null).Succes());
            Assert.Null(prefix.Match(null).RemainingText());
        }

        [Fact]
        public void PrefixAndTextIsEmpty()
        {
            var empty = new Text("");
            Assert.True(empty.Match("true").Succes());
            Assert.Equal("true", empty.Match("true").RemainingText());
        }

        [Fact]
        public void PrefixAndTextIsNull()
        {
            var empty = new Text("");
            Assert.False(empty.Match(null).Succes());
            Assert.Null(empty.Match(null).RemainingText());
        }*/
    }
}
