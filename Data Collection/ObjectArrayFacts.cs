using Xunit;

namespace DataCollection
{
    public class ObjectArrayFacts
    {
        [Fact]
        public void TestAddAndCount()
        {
            var objArray = new ObjectArray();
            objArray.Add(1);
            objArray.Add("sgsdhgjd");
            objArray.Add(-5);
            objArray.Add(6.501);
            objArray.Add('/');
            objArray.Add(7);
            objArray.Add(6);
            objArray.Add('s');
            objArray.Add(7);
            Assert.Equal(9, objArray.Count);
        }

        [Fact]
        public void TestElement()
        {
            var objArray = new ObjectArray();
            objArray.Add(1);
            objArray.Add("sgsdhgjd");
            objArray.Add(-5);
            objArray.Add(6.501);
            objArray.Add('/');
            objArray.Add(7);
            objArray.Add('s');
            Assert.Equal(1, objArray[0]);
            Assert.Equal("sgsdhgjd", objArray[1]);
            Assert.Equal(-5, objArray[2]);
            Assert.Equal(6.501, objArray[3]);
            Assert.Equal('/', objArray[4]);
            Assert.Equal(7, objArray[5]);
            Assert.Equal('s', objArray[6]);
        }

        [Fact]
        public void TestSetElement()
        {
            var objArray = new ObjectArray();
            objArray.Add(1);
            objArray.Add("sgsdhgjd");
            objArray.Add(-5);
            objArray.Add(6.501);
            objArray.Add('/');
            objArray[1] = 4;
            Assert.Equal(4, objArray[1]);
        }

        [Fact]
        public void Contains()
        {
            var objArray = new ObjectArray();
            objArray.Add(1);
            objArray.Add("sgsdhgjd");
            objArray.Add(-5);
            objArray.Add(6.501);
            objArray.Add('/');
            Assert.True(objArray.Contains(1));
            Assert.True(objArray.Contains("sgsdhgjd"));
            Assert.True(objArray.Contains(-5));
            Assert.True(objArray.Contains(6.501));
            Assert.True(objArray.Contains('/'));
        }

        [Fact]
        public void TestIndexOf()
        {
            var objArray = new ObjectArray();
            objArray.Add(1);
            objArray.Add("sgsdhgjd");
            objArray.Add(-5);
            objArray.Add(6.501);
            objArray.Add('/');
            Assert.Equal(0, objArray.IndexOf(1));
            Assert.Equal(1, objArray.IndexOf("sgsdhgjd"));
            Assert.Equal(2, objArray.IndexOf(-5));
            Assert.Equal(3, objArray.IndexOf(6.501));
            Assert.Equal(4, objArray.IndexOf('/'));
            Assert.Equal(-1, objArray.IndexOf(4));
        }

        [Fact]
        public void TestInsert()
        {
            var objArray = new ObjectArray();
            objArray.Add(1);
            objArray.Add("sgsdhgjd");
            objArray.Add(-5);
            objArray.Add(6.501);
            objArray.Add('/');
            objArray.Insert(1, 10);
            Assert.Equal(6, objArray.Count);
            Assert.Equal(1, objArray[0]);
            Assert.Equal(10, objArray[1]);
            Assert.Equal("sgsdhgjd", objArray[2]);
            Assert.Equal(-5, objArray[3]);
            Assert.Equal(6.501, objArray[4]);
            Assert.Equal('/', objArray[5]);
        }

        [Fact]
        public void TestClear()
        {
            var objArray = new ObjectArray();
            objArray.Add(1);
            objArray.Add("sgsdhgjd");
            objArray.Add(-5);
            objArray.Add(6.501);
            objArray.Add('/');
            objArray.Clear();
            Assert.Equal(null, objArray[0]);
        }

        [Fact]
        public void TestRemove()
        {
            var objArray = new ObjectArray();
            objArray.Add(1);
            objArray.Add("sgsdhgjd");
            objArray.Add(-5);
            objArray.Add(3);
            objArray.Add(6.501);
            objArray.Add('/');
            objArray.Remove(3);
            Assert.Equal(5, objArray.Count);
            objArray.Remove(6);
            Assert.Equal(5, objArray.Count);
            Assert.Equal(1, objArray[0]);
            Assert.Equal("sgsdhgjd", objArray[1]);
            Assert.Equal(-5, objArray[2]);
            Assert.Equal(6.501, objArray[3]);
            Assert.Equal('/', objArray[4]);
        }

        [Fact]
        public void TestRemoveAt()
        {
            var objArray = new ObjectArray();
            objArray.Add(1);
            objArray.Add("sgsdhgjd");
            objArray.Add(-5);
            objArray.Add(3);
            objArray.Add(6.501);
            objArray.Add('/');
            objArray.RemoveAt(3);
            Assert.Equal(5, objArray.Count);
            Assert.Equal(1, objArray[0]);
            Assert.Equal("sgsdhgjd", objArray[1]);
            Assert.Equal(-5, objArray[2]);
            Assert.Equal(6.501, objArray[3]);
            Assert.Equal('/', objArray[4]);
        }
    }
}
