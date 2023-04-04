using Xunit;

namespace DataCollection
{
    public class ObjIEnumeratorFacts
    {
        [Fact]
        public void GetEnumerator()
        {
            var objEnumerator = new ObjectArray { 1, -2, "fdfsdf", '/', 154.545 };
            var enumerator = objEnumerator.GetEnumerator();
            Assert.True(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(-2, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal("fdfsdf", enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal('/', enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(154.545, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Null(enumerator.Current);
        }
    }
}
