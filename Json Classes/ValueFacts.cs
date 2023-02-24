using Xunit;

namespace JsonClasses
{
    public class ValueFacts
    {
        [Fact]
        public void IsString()
        {
            var value = new Value();
            Assert.True(value.Match("\"input este un string json\"").Succes());
            Assert.Equal("", value.Match("\"input este un string json\"").RemainingText());
        }

        [Fact]
        public void IsNumber()
        {
            var value = new Value();
            Assert.True(value.Match("1123").Succes());
            Assert.Equal("", value.Match("1123").RemainingText());
        }

        [Fact]
        public void IsBoolean() 
        {
            var value = new Value();
            Assert.True(value.Match("true").Succes());
            Assert.Equal("", value.Match("true").RemainingText());
            Assert.True(value.Match("false").Succes());
            Assert.Equal("", value.Match("false").RemainingText());
        }

        [Fact]
        public void IsNull()
        {
            var value = new Value();
            Assert.True(value.Match("null").Succes());
            Assert.Equal("", value.Match("null").RemainingText());
        }

        [Fact]
        public void IsArray()
        {
            var value = new Value();
            Assert.True(value.Match("[]").Succes());
            Assert.Equal("", value.Match("[]").RemainingText());
            Assert.True(value.Match("[2, 3, 4, 124]").Succes());
            Assert.Equal("", value.Match("[2, 3, 4, 124]").RemainingText());
        }

        [Fact]
        public void IsObject() 
        {
            var value = new Value();
            Assert.True(value.Match("{ }").Succes());
            Assert.Equal("", value.Match("{ }").RemainingText());
            Assert.True(value.Match("{\"text\" : 54 }").Succes());
            Assert.Equal("", value.Match("{\"text\" : 54 }").RemainingText());
        }
    }
}
