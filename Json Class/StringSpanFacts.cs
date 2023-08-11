using Xunit;

namespace JsonClasses
{
    public class StringSpanFacts
    {
        [Fact]
        public void IsNullOrEmpty()
        {
            var actualResult1 = new StringSpan(string.Empty);
            Assert.True(actualResult1.IsNullOrEmpty());
            var actualResult2 = new StringSpan("dfff");
            Assert.False(actualResult2.IsNullOrEmpty());
        }

        [Fact]
        public void CharPeek()
        {
            string text = "dasdas";
            var actualResult = new StringSpan("dasdas");
            int position = 0;
            char peek = text[position];
            actualResult.CharPeek();
            Assert.Equal(peek, actualResult.CharPeek());
        }

        [Fact]
        public void Advance()
        {
            string text = "dasda";
            var actualResult = new StringSpan(text, 0);
            actualResult.Advance(2);
            var expectedResult = new StringSpan(text, 2);
            Assert.Equal(expectedResult.StartsWith("s"), actualResult.StartsWith("s"));
        }

        [Fact]
        public void StartsWith()
        {
            var actualResult = new StringSpan("sawq");
            string prefix = "sa";
            Assert.True(actualResult.StartsWith(prefix));
        }
    }
}
