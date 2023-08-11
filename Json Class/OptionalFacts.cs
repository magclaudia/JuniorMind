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
            var text = new StringSpan("abc");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("abc", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_TextContainLetterPrefixWhichIsRepeatedManyTimes()
        {
            var a = new Optional(new Character('a'));
            var text = new StringSpan("aabc");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("aabc", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_TextDoesNotContainPrefix()
        {
            var a = new Optional(new Character('a'));
            var text = new StringSpan("bc");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("bc", 0);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsEmpty()
        {
            var a = new Optional(new Character('a'));
            var text = new StringSpan("");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("", 0);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsNull()
        {
            var a = new Optional(new Character('a'));
            var text = new StringSpan(null);
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan(null, 0);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_TextContainSignPrefix()
        {
            var sign = new Optional(new Character('-'));
            var text1 = new StringSpan("123");
            var actualResult1 = sign.Match(text1);
            var expectedResult1 = new StringSpan("123", 0);
            Assert.True(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));

            var text2 = new StringSpan("-123");
            var actualResult2 = sign.Match(text2);
            var expectedResult2 = new StringSpan("-123", 1);
            Assert.True(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }
    }
}
