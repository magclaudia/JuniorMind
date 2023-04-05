using Xunit;

namespace DataCollection
{
    public class ObjIEnumeratorFacts
    {
        [Fact]
        public void GetEnumerator()
        {
            var objEnumerator = new ObjectArray();
            objEnumerator.Add(1);
            objEnumerator.Add(-2);
            objEnumerator.Add("fdfsdf");
            objEnumerator.Add('/');
            objEnumerator.Add(154.545);
            Assert.Equal(1, objEnumerator[0]);
            Assert.Equal(-2, objEnumerator[1]);
            Assert.Equal("fdfsdf", objEnumerator[2]);
            Assert.Equal('/', objEnumerator[3]);
            Assert.Equal(154.545, objEnumerator[4]);
        }
    }
}
