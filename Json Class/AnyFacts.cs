using Xunit;

namespace JsonClasses
{
    public class AnyFacts
    {
        [Fact]
        public void ValidString_StringStartWithRequiredChar()
        {
            var e = new Any("eE");
            var text = new StringSpan("Eadd");
            var actualResult = e.Match(text);
            var expectedResult = new StringSpan("Eadd", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void InvalidString()
        {
            var e = new Any("eE");
            var text = new StringSpan("gg");
            var actualResult = e.Match(text);
            var expectedResult = new StringSpan("gg", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void EmptyString()
        {
            var e = new Any("eE");
            var sign = new Any("-+");
            var text = new StringSpan("");
            var actualResult1 = e.Match(text);
            var expectedResult1 = new StringSpan("", 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));
            var actualResult2 = sign.Match(text);
            var expectedResult2 = new StringSpan("", 0);
            Assert.False(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }

        [Fact]
        public void NullString()
        {
            var e = new Any("eE");
            var sign = new Any("-+");
            var text = new StringSpan(null);
            var actualResult1 = e.Match(text);
            var expectedResult1 = new StringSpan(null, 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));
            var actualResult2 = sign.Match(text);
            var expectedResult2 = new StringSpan(null, 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }

        [Fact]
        public void ValidString_StartsWithRequiredSign()
        {
            var sign = new Any("-+");
            var text1 = new StringSpan("+3");
            var actualResult1 = sign.Match(text1);
            var expectedResult1 = new StringSpan("+3", 1);
            Assert.True(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));
            var text2 = new StringSpan("-3");
            var actualResult2 = sign.Match(text2);
            var expectedResult2 = new StringSpan("-3", 1);
            Assert.True(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }

        [Fact]
        public void InvalidString_DoesNotContainRequiredSign()
        {
            var sign = new Any("-+");
            var stringWrapper1 = new StringSpan("32");
            var actualResult1 = sign.Match(stringWrapper1);
            var expectedResult = new StringSpan("32", 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult1.RemainingText()));
        }
    }
}
