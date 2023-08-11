using Xunit;

namespace JsonClasses
{
    public class StringFacts
    {
        [Fact]
        public void IsWrappedInDoubleQuotes()
        {
            var stringJson = new String();
            var text = new StringSpan(Quoted("abc")); 
            var actualResult = stringJson.Match(text);
            var expectedResult = new StringSpan(Quoted("abc"), 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void AlwaysStartsWithQuotes()
        {
            var stringJson = new String();
            var text = new StringSpan(Quoted("abc\""));
            var actualResult = stringJson.Match(text);
            var expectedResult = new StringSpan(Quoted("abc\""), 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void AlwaysEndsWithQuotes()
        {
            var stringJson = new String();
            var text = new StringSpan(Quoted("\"abc"));
            var actualResult = stringJson.Match(text);
            var expectedResult = new StringSpan(Quoted("\"abc"), 2);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void IsNotNull()
        {
            var stringJson = new String();
            var text = new StringSpan(Quoted(null));
            var actualResult = stringJson.Match(text);
            var expectedResult = new StringSpan(Quoted(null), 2);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }


        [Fact]
        public void ShouldReturnEmptyStringIfStringIsEmpty()
        {
            var stringJson = new String();
            var text = new StringSpan(Quoted(""));
            var actualResult = stringJson.Match(text);
            var expectedResult = new StringSpan(Quoted(""), 2);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ShouldReturnEmptyIfStringContainsQuotationMark()
        {
            var stringJson = new String();
            var text = new StringSpan(Quoted(@"\""v\"" bp"));
            var actualResult = stringJson.Match(text);
            var expectedResult = new StringSpan(Quoted(@"\""v\"" bp"), 10);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void DoesNotContainControlCharacters()
        {
            var stringJson = new String();
            var text = new StringSpan(Quoted("a\nb\rc"));
            var actualResult = stringJson.Match(text);
            var expectedResult = new StringSpan(Quoted("a\nb\rc"), 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void CanContainLargeUnicodeCharacters()
        {
            var stringJson = new String();
            var text = new StringSpan(Quoted("⛅⚾"));
            var actualResult = stringJson.Match(text);
            var expectedResult = new StringSpan(Quoted("⛅⚾"), 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void DoesNotEndWithAnUnfinishedHexNumber()
        {
            var stringJson = new String();
            var text = new StringSpan(Quoted(@"a\u"));
            var actualResult = stringJson.Match(text);
            var expectedResult = new StringSpan(Quoted(@"a\u"), 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        public static string Quoted(string text)
           => $"\"{text}\"";
    }
}
