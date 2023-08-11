using Xunit;

namespace JsonClasses
{
    public class CharacterFacts
    {
        [Fact]
        public void StringStartsWithPattern()
        {
            Character c = new Character('0');
            var text = new StringSpan("0sd1");
            var actualResult = c.Match(text);
            var expectedResult = new StringSpan("0sd1", 1);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringDoesntStartsWithPattern()
        {
            Character c = new Character('1');
            var text = new StringSpan("dgahdg");
            var actualResult = c.Match(text);
            var expectedResult = new StringSpan("dgahdg", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsNull()
        {
            Character c = new('0');
            var text = new StringSpan(null);
            var actualResult = c.Match(text);
            var expectedResult = new StringSpan(null, 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void StringIsEmpty()
        {
            Character c = new('0');
            var text = new StringSpan("");
            var actualResult = c.Match(text);
            var expectedResult = new StringSpan("", 0);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }
    }
}
