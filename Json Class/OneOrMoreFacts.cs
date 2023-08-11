using JsonClasses;
using Xunit;

namespace JsonClasses
{
    public class OneOrMoreFacts
    {
        [Fact]
        public void ValidString_TextIsEqualWithPrefix()
        {
            var a = new OneOrMore(new Range('0', '9'));
            var text = new StringSpan("123");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("123", 3);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_TextContainPrefix()
        {
            var a = new OneOrMore(new Range('0', '9'));
            var text = new StringSpan("1a");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("1a", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_TextDoesNotContainPrefix()
        {
            var a = new OneOrMore(new Range('0', '9'));
            var text = new StringSpan("bc");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("bc", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsEmpty()
        {
            var a = new OneOrMore(new Range('0', '9'));
            var text = new StringSpan("");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsNull()
        {
            var a = new OneOrMore(new Range('0', '9'));
            var text = new StringSpan(null);
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan(null, 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }
    }
}
