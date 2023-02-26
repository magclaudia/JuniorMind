using JsonClasses;
using Xunit;

namespace JsonClasses
{
    public class ManyFacts
    {
        [Fact]
        public void ValidString_TextContainLetterPrefixWhichIsRepeatedOnce()
        {
            var a = new Many(new Character('a'));
            Assert.True(a.Match("abc").Succes());
            Assert.Equal("bc", a.Match("abc").RemainingText());
        }

        [Fact]
        public void ValidString_TextContainLetterPrefixWhichIsRepeatedManyTimes()
        {
            var a = new Many(new Character('a'));
            Assert.True(a.Match("aaaabc").Succes());
            Assert.Equal("bc", a.Match("aaaabc").RemainingText());
        }

        [Fact]
        public void ValidString_TextDoesNotContainPrefix()
        {
            var a = new Many(new Character('a'));
            Assert.True(a.Match("bc").Succes());
            Assert.Equal("bc", a.Match("bc").RemainingText());
        }

        [Fact]
        public void StringIsEmpty()
        {
            var a = new Many(new Character('a'));
            Assert.True(a.Match("").Succes());
            Assert.Equal("", a.Match("").RemainingText());
        }

        [Fact]
        public void StringIsNull()
        {
            var a = new Many(new Character('a'));
            Assert.True(a.Match(null).Succes());
            Assert.Null(a.Match(null).RemainingText());
        }

        [Fact]
        public void ValidString_TextContainDigitPrefix()
        {
            var digits = new Many(new Range('0', '9'));
            Assert.True(digits.Match("12345ab123").Succes());
            Assert.Equal("ab123", digits.Match("12345ab123").RemainingText());
            Assert.True(digits.Match("ab").Succes());
            Assert.Equal("ab", digits.Match("ab").RemainingText());
        }
    }
}
