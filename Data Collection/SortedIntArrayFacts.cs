using Xunit;

namespace DataCollection
{
    public class SortedIntArrayFacts
    {
        [Fact]
        public void TestAddAndCount()
        {
            var sortedArray = new SortedIntArray();
            sortedArray.Add(1);
            sortedArray.Add(3);
            sortedArray.Add(5);
            sortedArray.Add(6);
            sortedArray.Add(2);
            sortedArray.Add(7);
            Assert.Equal(6, sortedArray.Count);
            Assert.Equal(5, sortedArray[3]);
            Assert.Equal(1, sortedArray[0]);
        }

        [Fact]
        public void TestElement()
        {
            var sortedArray = new SortedIntArray();
            sortedArray.Add(1);
            sortedArray.Add(2);
            sortedArray.Add(3);
            sortedArray.Add(1);
            sortedArray.Add(3);
            sortedArray.Add(5);
            sortedArray.Add(6);
            Assert.Equal(1, sortedArray[0]);
            Assert.Equal(1, sortedArray[1]);
            Assert.Equal(2, sortedArray[2]);
            Assert.Equal(3, sortedArray[3]);
            Assert.Equal(3, sortedArray[4]);
            Assert.Equal(5, sortedArray[5]);
            Assert.Equal(6, sortedArray[6]);
        }

        [Fact]
        public void TestSetElementWhenElementIsSetToCorrenctIndex()
        {
            var sortedArray = new SortedIntArray();
            sortedArray.Add(3);
            sortedArray.Add(5);
            sortedArray.Add(7);
            sortedArray[1] = 4;
            Assert.Equal(4, sortedArray[1]);
        }

        [Fact]
        public void TestSetElementWhenElementIsNotSetToCorrenctIndex()
        {
            var sortedArray = new SortedIntArray();
            sortedArray.Add(2);
            sortedArray.Add(5);
            sortedArray.Add(9);
            sortedArray.Add(4);
            sortedArray[0] = 10;
            Assert.Equal(2, sortedArray[0]);
        }

        [Fact]
        public void Contains()
        {
            var sortedArray = new SortedIntArray();
            sortedArray.Add(1);
            sortedArray.Add(2);
            sortedArray.Add(3);
            sortedArray.Add(3);
            sortedArray.Add(5);
            sortedArray.Add(6);
            sortedArray[3] = 10;
            Assert.True(sortedArray.Contains(1));
            Assert.True(sortedArray.Contains(2));
            Assert.True(sortedArray.Contains(3));
            Assert.False(sortedArray.Contains(10));
        }

        [Fact]
        public void TestIndexOf()
        {
            var sortedArray = new SortedIntArray();
            sortedArray.Add(1);
            sortedArray.Add(2);
            sortedArray.Add(3);
            Assert.Equal(0, sortedArray.IndexOf(1));
            Assert.Equal(1, sortedArray.IndexOf(2));
            Assert.Equal(2, sortedArray.IndexOf(3));
            Assert.Equal(-1, sortedArray.IndexOf(4));
        }

        [Fact]
        public void TestInsert()
        {
            var sortedArray = new SortedIntArray();
            sortedArray.Add(3);
            sortedArray.Add(6);
            sortedArray.Add(9);
            sortedArray.Add(17);
            sortedArray.Add(15);
            sortedArray.Insert(1, 5);
            Assert.Equal(6, sortedArray.Count);
            Assert.True(sortedArray.Contains(5));
            Assert.Equal(1, sortedArray.IndexOf(5));
            sortedArray.Insert(0, 18);
            Assert.Equal(6, sortedArray.Count);
            Assert.False(sortedArray.Contains(18));
        }

        [Fact]
        public void TestClear()
        {
            var sortedArray = new SortedIntArray();
            sortedArray.Add(1);
            sortedArray.Add(2);
            sortedArray.Add(3);
            sortedArray.Clear();
            Assert.Equal(0, sortedArray.Count);
            Assert.False(sortedArray.Contains(1));
        }

        [Fact]
        public void TestRemove()
        {
            var sortedArray = new SortedIntArray();
            sortedArray.Add(1);
            sortedArray.Add(6);
            sortedArray.Add(3);
            sortedArray.Add(5);
            sortedArray.Add(2);
            sortedArray.Remove(3);
            Assert.Equal(4, sortedArray.Count);
            Assert.False(sortedArray.Contains(3));
            sortedArray.Remove(7);
            Assert.Equal(4, sortedArray.Count);
            Assert.False(sortedArray.Contains(7));
            sortedArray[2] = 4;
            sortedArray.Remove(4);
            Assert.Equal(3, sortedArray.Count);
            Assert.False(sortedArray.Contains(4));
            Assert.Equal(1, sortedArray[0]);
            Assert.Equal(2, sortedArray[1]);
            Assert.Equal(6, sortedArray[2]);
        }

        [Fact]
        public void TestRemoveAt()
        {
            var sortedArray = new SortedIntArray();
            sortedArray.Add(7);
            sortedArray.Add(3);
            sortedArray.Add(5);
            sortedArray.Add(7);
            sortedArray.Add(6);
            sortedArray.Add(12);
            sortedArray.RemoveAt(3);
            sortedArray.Add(10);
            sortedArray[0] = 2;
            sortedArray[1] = 8;
            Assert.Equal(2, sortedArray[0]);
            Assert.False(sortedArray.Contains(8));
            sortedArray.RemoveAt(0);
            Assert.Equal(5, sortedArray.Count);
            Assert.Equal(2, sortedArray.IndexOf(7));
            Assert.Equal(3, sortedArray.IndexOf(10));
        }
    }
}