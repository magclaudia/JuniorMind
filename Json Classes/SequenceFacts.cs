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
            Assert.True(ab.Match("abcd").Succes());
            Assert.Equal("cd", ab.Match("abcd").RemainingText());
            Assert.True(abc.Match("abcd").Succes());
            Assert.Equal("d", abc.Match("abcd").RemainingText());
        }

        [Fact]
        public void InvalidString_DoesNotHaveAllRequiredChars()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            var abc = new Sequence(ab, new Character('c'));
            Assert.False(ab.Match("ax").Succes());
            Assert.Equal("ax", ab.Match("ax").RemainingText());
            Assert.False(abc.Match("abx").Succes());
            Assert.Equal("abx", abc.Match("abx").RemainingText());
        }

        [Fact]
        public void InvalidString_DoesNotHaveRequiredChars()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            Assert.False(ab.Match("def").Succes());
            Assert.Equal("def", ab.Match("def").RemainingText());
        }

        [Fact]
        public void InvalidString_DoesNotBeginRequiredChar()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            Assert.False(ab.Match("dab").Succes());
            Assert.Equal("dab", ab.Match("dab").RemainingText());
        }

        [Fact]
        public void InvalidStringForEmptyString()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            var abc = new Sequence(ab, new Character('c'));
            Assert.False(ab.Match("").Succes());
            Assert.Equal("", ab.Match("").RemainingText());
            Assert.False(abc.Match("").Succes());
            Assert.Equal("", abc.Match("").RemainingText());
        }

        [Fact]
        public void InvalidStringForNull()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            var abc = new Sequence(ab, new Character('c'));
            Assert.False(ab.Match(null).Succes());
            Assert.Null(ab.Match(null).RemainingText());
            Assert.False(abc.Match(null).Succes());
            Assert.Null(abc.Match(null).RemainingText());
        }

        [Fact]
        public void ValidHexFormatWithoutAnyRemainingChars()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            Assert.True(hexSeq.Match("u1234").Succes());
            Assert.Equal("", hexSeq.Match("u1234").RemainingText());
        }

        [Fact] public void ValidHexFormatWithRemainingChars() 
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            Assert.True(hexSeq.Match("uabcdef").Succes());
            Assert.Equal("ef", hexSeq.Match("uabcdef").RemainingText());
        }

        [Fact]
        public void ValidHexFormatWithRemainingCharsSplitBySpace()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            Assert.True(hexSeq.Match("uB005 ab").Succes());
            Assert.Equal(" ab", hexSeq.Match("uB005 ab").RemainingText());
        }

        [Fact]
        public void InvalidHexFormat()
        {
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            Assert.False(hexSeq.Match("abc").Succes());
            Assert.Equal("abc", hexSeq.Match("abc").RemainingText());
            Assert.False(hexSeq.Match(null).Succes());
            Assert.Null(hexSeq.Match(null).RemainingText());
        }
    }
}
