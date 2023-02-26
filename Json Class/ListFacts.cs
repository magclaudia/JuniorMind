using JsonClasses;
using Xunit;

namespace JsonClasses
{
    public class ListFacts
    {
        [Fact]
        public void ValidString_StringHasAllElementsInRequiredInterval()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("1,2,3").Succes());
            Assert.Equal("", a.Match("1,2,3").RemainingText());
        }

        [Fact]
        public void ValidString_StringHasAllElementsInRequiredIntervalAndRemainingCharacters()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("1,2,3,").Succes());
            Assert.Equal(",", a.Match("1,2,3,").RemainingText());
        }

        [Fact]
        public void ValidString_StringhasAllElementsIn()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("1a").Succes());
            Assert.Equal("a", a.Match("1a").RemainingText());
        }

        [Fact]
        public void ValidString_StringhasAllElements()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("abc").Succes());
            Assert.Equal("abc", a.Match("abc").RemainingText());
        }

        [Fact]
        public void ValidString_StringhasOnlyElementsWithoutSeparator()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("1234567").Succes());
            Assert.Equal("234567", a.Match("1234567").RemainingText());
        }
    }
}
