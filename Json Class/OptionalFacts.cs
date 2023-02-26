using JsonClasses;
using Xunit;

namespace JsonClasses
{
    public class OptionalFacts
    {
        [Fact]
        public void ValidString_TextContainLetterPrefix()
        {
            var a = new Optional(new Character('a'));
            Assert.True(a.Match("abc").Succes());
            Assert.Equal("bc", a.Match("abc").RemainingText());
        }

        [Fact]
        public void ValidString_TextContainLetterPrefixWhichIsRepeatedManyTimes()
        {
            var a = new Optional(new Character('a'));
            Assert.True(a.Match("aabc").Succes());
            Assert.Equal("abc", a.Match("aabc").RemainingText());
        }

        [Fact]
        public void ValidString_TextDoesNotContainPrefix()
        {
            var a = new Optional(new Character('a'));
            Assert.True(a.Match("bc").Succes());
            Assert.Equal("bc", a.Match("bc").RemainingText());
        }

        [Fact]
        public void StringIsEmpty()
        {
            var a = new Optional(new Character('a'));
            Assert.True(a.Match("").Succes());
            Assert.Equal("", a.Match("").RemainingText());
        }

        [Fact]
        public void StringIsNull()
        {
            var a = new Optional(new Character('a'));
            Assert.True(a.Match(null).Succes());
            Assert.Null(a.Match(null).RemainingText());
        }

        [Fact]
        public void ValidString_TextContainSignPrefix()
        {
            var sign = new Optional(new Character('-'));
            Assert.True(sign.Match("123").Succes());
            Assert.Equal("123", sign.Match("123").RemainingText());
            Assert.True(sign.Match("-123").Succes());
            Assert.Equal("123", sign.Match("-123").RemainingText());
        }
    }
}
