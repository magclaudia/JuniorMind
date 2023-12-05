using JsonClasses;
using Xunit;

namespace JsonClasses
{
    public class ListFacts
    {
        [Fact]
        public void ValidString_StringHasAllElementsInRequiredInterval()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            var text = new StringSpan("1,2,3");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("1,2,3", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_StringHasAllElementsInRequiredIntervalAndRemainingCharacters()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            var text = new StringSpan("1,2,");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("1,2,", 3);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_StringhasAllElementsIn()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            var text = new StringSpan("1a");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("1a", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_StringhasAllElements()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            var text = new StringSpan("abc");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("abc", 0);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidString_StringhasOnlyElementsWithoutSeparator()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            var text = new StringSpan("1234567");
            var actualResult = a.Match(text);
            var expectedResult = new StringSpan("1234567", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }
        
        [Fact]
        public void TestListDoNotConsume()
        {
            var element = new String();
            var separator = new Character(',');
            var list = new List(element, separator);
            var text = new StringSpan(" ");
            var actualResult = list.Match(text);
            Assert.True(actualResult.Succes());
            Assert.True(text.CheckIfEqualTo(actualResult.RemainingText()));
        }
    }
}
