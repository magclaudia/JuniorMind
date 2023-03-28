using Xunit;

namespace DataCollection
{
    public class IntArrayFacts
    {
        public class IntArrayTests
        {
            [Fact]
            public void TestxAddAndCount()
            {
                IntArray intArray = new IntArray();
                intArray.Add(1);
                intArray.Add(3);
                intArray.Add(5);
                intArray.Add(6);
                intArray.Add(2);
                intArray.Add(7);
                intArray.Add(6);
                intArray.Add(2);
                intArray.Add(7);
                Assert.Equal(9, intArray.Count);
            }

            [Fact]
            public void TestElement()
            {
                IntArray intArray = new IntArray();
                intArray.Add(1);
                intArray.Add(2);
                intArray.Add(3);
                intArray.Add(1);
                intArray.Add(3);
                intArray.Add(5);
                intArray.Add(6);
                Assert.Equal(1, intArray.Element(0));
                Assert.Equal(2, intArray.Element(1));
                Assert.Equal(3, intArray.Element(2));
                Assert.Equal(1, intArray.Element(3));
                Assert.Equal(3, intArray.Element(4));
                Assert.Equal(5, intArray.Element(5));
                Assert.Equal(6, intArray.Element(6));
            }

            [Fact]
            public void TestSetElement()
            {
                IntArray intArray = new IntArray();
                intArray.Add(1);
                intArray.Add(2);
                intArray.Add(3);
                intArray.SetElement(1, 4);
                Assert.Equal(4, intArray.Element(1));
            }

            [Fact]
            public void Contains()
            {
                IntArray intArray = new IntArray();
                intArray.Add(1);
                intArray.Add(2);
                intArray.Add(3);
                intArray.Add(3);
                intArray.Add(5);
                intArray.Add(6);
                Assert.True(intArray.Contains(1));
                Assert.True(intArray.Contains(2));
                Assert.True(intArray.Contains(3));
            }

            [Fact]
            public void TestIndexOf()
            {
                IntArray intArray = new IntArray();
                intArray.Add(1);
                intArray.Add(2);
                intArray.Add(3);
                Assert.Equal(0, intArray.IndexOf(1));
                Assert.Equal(1, intArray.IndexOf(2));
                Assert.Equal(2, intArray.IndexOf(3));
                Assert.Equal(-1, intArray.IndexOf(4));
            }

            [Fact]
            public void TestInsert()
            {
                IntArray intArray = new IntArray();
                intArray.Add(1);
                intArray.Add(2);
                intArray.Add(3);
                intArray.Add(4);
                intArray.Add(5);
                intArray.Add(6);
                intArray.Add(7);
                intArray.Add(8);
                intArray.Insert(1, 10);
                Assert.Equal(9, intArray.Count);
                Assert.Equal(0, intArray.IndexOf(1));
                Assert.Equal(1, intArray.IndexOf(10));
                Assert.Equal(2, intArray.IndexOf(2));
                Assert.Equal(3, intArray.IndexOf(3));
                Assert.Equal(4, intArray.IndexOf(4));
                Assert.Equal(5, intArray.IndexOf(5));
                Assert.Equal(6, intArray.IndexOf(6));
                Assert.Equal(7, intArray.IndexOf(7));
                Assert.Equal(8, intArray.IndexOf(8));

            }

            [Fact]
            public void TestClear()
            {
                IntArray intArray = new IntArray();
                intArray.Add(1);
                intArray.Add(2);
                intArray.Add(3);
                intArray.Clear();
                Assert.Equal(0, intArray.IndexOf(0));
            }

            [Fact]
            public void TestRemove()
            {
                IntArray intArray = new IntArray();
                intArray.Add(1);
                intArray.Add(2);
                intArray.Add(3);
                intArray.Add(5);
                intArray.Add(3);
                intArray.Remove(3);
                Assert.Equal(4, intArray.Count);
                intArray.Remove(6);
                Assert.Equal(4, intArray.Count);
                Assert.Equal(0, intArray.IndexOf(1));
                Assert.Equal(1, intArray.IndexOf(2));
                Assert.Equal(2, intArray.IndexOf(5));
                Assert.Equal(3, intArray.IndexOf(3));
            }

            [Fact]
            public void TestRemoveAt()
            {
                IntArray intArray = new IntArray();
                intArray.Add(1);
                intArray.Add(2);
                intArray.Add(3);
                intArray.Add(4);
                intArray.Add(3);
                intArray.RemoveAt(4);
                Assert.Equal(4, intArray.Count);
                Assert.Equal(0, intArray.IndexOf(1));
                Assert.Equal(1, intArray.IndexOf(2));
                Assert.Equal(2, intArray.IndexOf(3));
                Assert.Equal(3, intArray.IndexOf(4));
            }
        }
    }
}
