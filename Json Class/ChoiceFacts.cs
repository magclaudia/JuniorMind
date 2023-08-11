using Xunit;

namespace JsonClasses
{
    public class ChoiceFacts
    {
        [Fact]
        public void ValidString_StringStartsWithRequiredChar()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            var text1 = new StringSpan("0");
            var actualResult1 = digit.Match(text1);
            var expectedResult = new StringSpan("0", 1);
            Assert.True(actualResult1.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult1.RemainingText()));
            
            var text2 = new StringSpan("a2314");
            var actualResult2 = hex.Match(text2);
            var expectedResult2 = new StringSpan("a2314", 1);
            Assert.True(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));

            var text3 = new StringSpan("A2314");
            var actualResult3 = hex.Match(text3);
            var expectedResult3 = new StringSpan("A2314", 1);
            Assert.True(actualResult3.Succes());
            Assert.True(expectedResult3.CheckIfEqualTo(actualResult3.RemainingText()));
        }


        [Fact]
        public void StringInvalid_StringDoesNotStartWithRequiredChar()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            var text1 = new StringSpan("a1247");
            var actualResult1 = digit.Match(text1);
            var expectedResult1 = new StringSpan("a1247", 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));
            
            var text2 = new StringSpan("z1245");
            var actualResult2 = hex.Match(text2);
            var expectedResult2 = new StringSpan("z1245", 0);
            Assert.False(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }

        [Fact]
        public void StringIsNull()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            var text1 = new StringSpan(null);
            var actualResult1 = digit.Match(text1);
            var expectedResult1 = new StringSpan(null, 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));

            var text2 = new StringSpan(null);
            var actualResult2 = hex.Match(text2);
            var expectedResult2 = new StringSpan(null, 0);
            Assert.False(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }

        [Fact]
        public void StringIsEmpty()
        {
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            var hex = new Choice(digit, new Range('a', 'f'), new Range('A', 'F'));
            var text1 = new StringSpan("");
            var actualResult1 = digit.Match(text1);
            var expectedResult1 = new StringSpan("", 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));

            var text2 = new StringSpan("");
            var actualResult2 = hex.Match(text2);
            var expectedResult2 = new StringSpan("", 0);
            Assert.False(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }
    }
}
