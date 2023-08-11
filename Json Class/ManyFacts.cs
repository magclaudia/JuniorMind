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
            var text = new StringSpan("abc");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("abc", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_TextContainLetterPrefixWhichIsRepeatedManyTimes()
        {
            var a = new Many(new Character('a'));
            var text = new StringSpan("aaaabc");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("aaaabc", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_TextDoesNotContainPrefix()
        {
            var a = new Many(new Character('a'));
            var text = new StringSpan("bc");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("bc", 0);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsEmpty()
        {
            var a = new Many(new Character('a'));
            var text = new StringSpan("");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("", 0);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsNull()
        {
            var a = new Many(new Character('a'));
            var text = new StringSpan(null);
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan(null, 0);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_TextContainDigitPrefix()
        {
            var digits = new Many(new Range('0', '9'));
            var text = new StringSpan("12345ab123");
            var actualResult = digits.Match(text);
            var expectedResult = new StringSpan("12345ab123", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }
    }
}
