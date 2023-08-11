using Xunit;

namespace JsonClasses
{
    public class RangeFacts
    {
        [Fact]
        public void StringStartsWithFirstCharFromRange()
        {
            Range digit = new Range('a', 'f');
            var text = new StringSpan("abc");
            var actualResult = digit.Match(text);
            var expectedResult = new StringSpan("abc", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringStartsWithLastCharFromRange()
        {
            Range digit = new Range('a', 'f');
            var text = new StringSpan("fab");
            var actualResult = digit.Match(text);
            var expectedResult = new StringSpan("fab", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringStartsWithACharThatIsInRange()
        {
            Range digit = new Range('a', 'f');
            var text = new StringSpan("bcd");
            var actualResult = digit.Match(text);
            var expectedResult = new StringSpan("bcd", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringStartsWithACharThatIsNotInRange()
        {
            Range digit = new Range('a', 'f');
            var text = new StringSpan("1ab");
            var actualResult = digit.Match(text);
            var expectedResult = new StringSpan("1ab", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsNull()
        {
            Range digit = new Range('a', 'f');
            var text = new StringSpan(null);
            var actualResult = digit.Match(text);
            var expectedResult = new StringSpan(null, 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsEmpty()
        {
            Range digit = new Range('a', 'f');
            var text = new StringSpan("");
            var actualResult = digit.Match(text);
            var expectedResult = new StringSpan("", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }
    }
}
