using Xunit;

namespace JsonClasses
{
    public class TextFacts
    {
        [Fact]
        public void ValidString_StringHasRequiredPrefix()
        {
            var prefix = new Text("true");
            var text = new StringSpan("true");
            var actualResult = prefix.Match(text);
            var expectedResult = new StringSpan("true", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_StringHasRequiredPrefixAndRemainigText()
        {
            var prefix = new Text("true");
            var text = new StringSpan("trueX");
            var actualResult = prefix.Match(text);
            var expectedResult = new StringSpan("trueX", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void InvalidString_StringHasRequiredPrefixAndRemainigText()
        {
            var prefix = new Text("true");
            var text = new StringSpan("false");
            var actualResult = prefix.Match(text);
            var expectedResult = new StringSpan("false", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsEmptyString()
        {
            var prefix = new Text("true");
            var text = new StringSpan("");
            var actualResult = prefix.Match(text);
            var expectedResult = new StringSpan("", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsNull()
        {
            var prefix = new Text("true");
            var text = new StringSpan(null);
            var actualResult = prefix.Match(text);
            var expectedResult = new StringSpan(null, 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void PrefixAndTextIsEmpty()
        {
            var empty = new Text("");
            var text = new StringSpan("true");
            var actualResult = empty.Match(text);
            var expectedResult = new StringSpan("true", 0);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void PrefixAndTextIsNull()
        {
            var empty = new Text("");
            var text = new StringSpan(null);
            var actualResult = empty.Match(text);
            var expectedResult = new StringSpan(null, 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }
    }
}
