using Xunit;

namespace JsonClasses
{
    public class StringFacts
    {
        /*[Fact]
        public void IsWrappedInDoubleQuotes()
        {
            var stringJson = new String();
            Assert.True(stringJson.Match(Quoted("abc")).Succes());
            Assert.Equal("", stringJson.Match(Quoted("abc")).RemainingText());
        }

        [Fact]
        public void AlwaysStartsWithQuotes()
        {
            var stringJson = new String();
            Assert.True(stringJson.Match(Quoted("abc\"")).Succes());
            Assert.Equal("\"", stringJson.Match(Quoted("abc\"")).RemainingText());
        }

        [Fact]
        public void AlwaysEndsWithQuotes()
        {
            var stringJson = new String();
            Assert.True(stringJson.Match(Quoted("\"abc")).Succes());
            Assert.Equal("abc\"", stringJson.Match(Quoted("\"abc")).RemainingText());
        }

        [Fact]
        public void IsNotNull()
        {
            var stringJson = new String();
            Assert.True(stringJson.Match(Quoted(null)).Succes());
            Assert.Equal("", stringJson.Match(Quoted(null)).RemainingText());
        }


        [Fact]
        public void ShouldReturnEmptyStringIfStringIsEmpty()
        {
            var stringJson = new String();
            Assert.True(stringJson.Match(Quoted("")).Succes());
            Assert.Equal("", stringJson.Match(Quoted("")).RemainingText());
        }

        [Fact]
        public void ShouldReturnEmptyIfStringContainsQuotationMark()
        {
            var stringJson = new String();
            Assert.True(stringJson.Match(Quoted(@"\""v\"" bp")).Succes());
            Assert.Equal("", stringJson.Match(Quoted(@"\""v\"" bq")).RemainingText());
        }

        [Fact]
        public void DoesNotContainControlCharacters()
        {
            var stringJson = new String();
            Assert.False(stringJson.Match(Quoted("a\nb\rc")).Succes());
            Assert.Equal("\"a\nb\rc\"", stringJson.Match(Quoted("a\nb\rc")).RemainingText());
        }

        [Fact]
        public void CanContainLargeUnicodeCharacters()
        {
            var stringJson = new String();
            Assert.True(stringJson.Match(Quoted("⛅⚾")).Succes());
            Assert.Equal("", stringJson.Match(Quoted("⛅⚾")).RemainingText());
        }

        [Fact]
        public void DoesNotEndWithAnUnfinishedHexNumber()
        {
            var stringJson = new String();
            Assert.False(stringJson.Match(Quoted(@"a\u")).Succes());
            Assert.Equal("\"a\\u\"", stringJson.Match(Quoted(@"a\u")).RemainingText());
        }

        public static string Quoted(string text)
           => $"\"{text}\"";*/
    }
}
