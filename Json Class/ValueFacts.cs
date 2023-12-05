using Xunit;

namespace JsonClasses
{
    public class ValueFacts
    {
        [Fact]
        public void IsString()
        {
            var value = new Value();
            var text = new StringSpan("\"input este un string json\"");
            var actualResult = value.Match(text);
            var expectedResult = new StringSpan("\"input este un string json\"", 27);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void IsNumber()
        {
            var value = new Value();
            var text = new StringSpan("1123");
            var actualResult = value.Match(text);
            var expectedResult = new StringSpan("1123", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void IsBoolean()
        {
            var value = new Value();
            var text1 = new StringSpan("true");
            var actualResult1 = value.Match(text1);
            var expectedResult1 = new StringSpan("true", 4);
            Assert.True(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));

            var text = new StringSpan("false");
            var actualResult = value.Match(text);
            var expectedResult = new StringSpan("false", 5);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void IsNull()
        {
            var value = new Value();
            var text = new StringSpan("null");
            var actualResult = value.Match(text);
            var expectedResult = new StringSpan("null", 4);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void IsArray()
        {
            var value = new Value();
            var text = new StringSpan("[2, 3, 4, 124]");
            var actualResult = value.Match(text);
            var expectedResult = new StringSpan("[2, 3, 4, 124]", 14);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void IsObject()
        {
            var value = new Value();
            var text = new StringSpan("{\"text\" : 54 }");
            var actualResult = value.Match(text);
            var expectedResult = new StringSpan("{\"text\" : 54 }", 14);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void EmptyArrayIsAValidFormat()
        {
            var value = new Value();
            var text = new StringSpan("[ ]");
            var actualResult = value.Match(text);
            var expectedResult = new StringSpan("[ ]", 3);
            Assert.True(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
        }

        [Fact]
        public void EmptyObjectIsAValidFormat()
        {
            var value = new Value();
            var text1 = new StringSpan("{ }");
            var actualResult1 = value.Match(text1);
            var expectedResult1 = new StringSpan("{ }", 3);
            Assert.True(actualResult1.Succes());
            Assert.True(expectedResult1.CheckIfEqualTo(actualResult1.RemainingText()));
        }


        [Fact]
        public void JustOneParenthes_isInvalidFormat_ReturnInvalidFormat_WeAddedAVerificationForActualMatchSuccesInMain()
        {
            var value = new Value();
            var text1 = new StringSpan("{");
            var actualResult = value.Match(text1);
            var expectedResult = new StringSpan("{", 1);
            var errorLocation = actualResult.RemainingText().GetLineAndColumnOfPosition();
            var expectedLocation = (1, 2);
            Assert.False(actualResult.Succes());
            Assert.True(expectedResult.CheckIfEqualTo(actualResult.RemainingText()));
            Assert.Equal(expectedLocation, errorLocation);
        }
    }
}
