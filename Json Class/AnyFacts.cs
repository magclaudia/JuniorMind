using Xunit;

namespace JsonClasses
{
    public class AnyFacts
    {
        /*[Fact]
        public void ValidString_StringStartWithRequiredChar()
        {
            var e = new Any("eE");
            Assert.True(e.Match("ea").Succes());
            Assert.Equal("a", e.Match("ea").RemainingText());
            Assert.True(e.Match("Ea").Succes());
            Assert.Equal("a", e.Match("Ea").RemainingText());
        }

        [Fact]
        public void InvalidString()
        {
            var e = new Any("eE");
            Assert.False(e.Match("a").Succes());
            Assert.Equal("a", e.Match("a").RemainingText());
        }

        [Fact]
        public void EmptyString()
        {
            var e = new Any("eE");
            var sign = new Any("-+");
            Assert.False(e.Match("").Succes());
            Assert.Equal("", e.Match("").RemainingText());
            Assert.False(sign.Match("").Succes());
            Assert.Equal("", sign.Match("").RemainingText());
        }

        [Fact]
        public void NullString()
        {
            var e = new Any("eE");
            var sign = new Any("-+");
            Assert.False(e.Match(null).Succes());
            Assert.Null(e.Match(null).RemainingText());
            Assert.False(sign.Match(null).Succes());
            Assert.Null(sign.Match(null).RemainingText());
        }

        [Fact]
        public void ValidString_StartsWithRequiredSign()
        {
            var sign = new Any("-+");
            Assert.True(sign.Match("+3").Succes());
            Assert.Equal("3", sign.Match("+3").RemainingText());
            Assert.True(sign.Match("-2").Succes());
            Assert.Equal("2", sign.Match("-2").RemainingText());
        }

        [Fact]
        public void InvalidString_DoesNotContainRequiredSign()
        {
            var sign = new Any("-+");
            Assert.False(sign.Match("2").Succes());
            Assert.Equal("2", sign.Match("2").RemainingText());
        }*/
    }
}
