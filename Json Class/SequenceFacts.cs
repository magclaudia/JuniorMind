using JsonClasses;
using Xunit;

namespace JsonClasses
{
    public class SequenceFacts
    {
        [Fact]
        public void ValidString_BeginWithRightCharsAndHasLeftCharsAndReturnManyCharsAfter()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            var abc = new Sequence(ab, new Character('c'));
            var text1 = new StringSpan("abcd");
            var actualResult1 = ab.Match(text1);
            var expectedResult1 = new StringSpan("abcd", 2);
            Assert.True(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));

            var text2 = new StringSpan("abcd");
            var actualResult2 = abc.Match(text2);
            var expectedResult2 = new StringSpan("abcd", 3);
            Assert.True(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }

        [Fact]
        public void InvalidString_DoesNotHaveAllRequiredChars()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            var abc = new Sequence(ab, new Character('c'));
            var text1 = new StringSpan("ax");
            var actualResult1 = ab.Match(text1);
            var expectedResult1 = new StringSpan("ax", 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));

            var text2 = new StringSpan("abx");
            var actualResult2 = abc.Match(text2);
            var expectedResult2 = new StringSpan("abx", 0);
            Assert.False(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }

        [Fact]
        public void InvalidString_DoesNotHaveRequiredChars()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            var text = new StringSpan("def");
            var actualResult = ab.Match(text);
            var expectedResult = new StringSpan("def", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void InvalidString_DoesNotBeginRequiredChar()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            var text = new StringSpan("dab");
            var actualResult = ab.Match(text);
            var expectedResult = new StringSpan("dab", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void InvalidStringForEmptyString()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            var abc = new Sequence(ab, new Character('c'));
            var text1 = new StringSpan("");
            var actualResult1 = ab.Match(text1);
            var expectedResult1 = new StringSpan("", 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));

            var text2 = new StringSpan("");
            var actualResult2 = abc.Match(text2);
            var expectedResult2 = new StringSpan("", 0);
            Assert.False(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }

        [Fact]
        public void InvalidStringForNull()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            var abc = new Sequence(ab, new Character('c'));
            var text1 = new StringSpan(null);
            var actualResult1 = ab.Match(text1);
            var expectedResult1 = new StringSpan(null, 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));

            var text2 = new StringSpan(null);
            var actualResult2 = abc.Match(text2);
            var expectedResult2 = new StringSpan(null, 0);
            Assert.False(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }

        [Fact]
        public void ValidHexFormatWithoutAnyRemainingChars()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            var text = new StringSpan("u1234");
            var actualResult = hexSeq.Match(text);
            var expectedResult = new StringSpan("u1234", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidHexFormatWithRemainingChars()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            var text = new StringSpan("uabcdef");
            var actualResult = hexSeq.Match(text);
            var expectedResult = new StringSpan("uabcdef", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void ValidHexFormatWithRemainingCharsSplitBySpace()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            var text = new StringSpan("uB005 ab");
            var actualResult = hexSeq.Match(text);
            var expectedResult = new StringSpan("uB005 ab", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void InvalidHexFormat()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            var text1 = new StringSpan("abc");
            var actualResult1 = hexSeq.Match(text1);
            var expectedResult1 = new StringSpan("abc", 0);
            Assert.False(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));

            var text2 = new StringSpan(null);
            var actualResult2 = hexSeq.Match(text2);
            var expectedResult2 = new StringSpan(null, 0);
            Assert.False(actualResult2.Succes());
            Assert.True(expectedResult2.CheckIfEqualTo(actualResult2.RemainingText()));
        }
    }
}
