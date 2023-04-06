using Xunit;

namespace DataCollection
{
    public class SortedListFacts
    {
        [Fact]
        public void TestAddAndCount()
        {
            var sortList = new SortedList<int>();
            sortList.Add(1);
            sortList.Add(3);
            sortList.Add(5);
            sortList.Add(6);
            sortList.Add(2);
            sortList.Add(7);
            Assert.Equal(6, sortList.Count);
            Assert.Equal(5, sortList[3]);
            Assert.Equal(1, sortList[0]);
        }

        [Fact]
        public void TestElement()
        {
            var sortList = new SortedList<int>();
            sortList.Add(1);
            sortList.Add(2);
            sortList.Add(3);
            sortList.Add(1);
            sortList.Add(3);
            sortList.Add(5);
            sortList.Add(6);
            Assert.Equal(1, sortList[0]);
            Assert.Equal(1, sortList[1]);
            Assert.Equal(2, sortList[2]);
            Assert.Equal(3, sortList[3]);
            Assert.Equal(3, sortList[4]);
            Assert.Equal(5, sortList[5]);
            Assert.Equal(6, sortList[6]);
        }

        [Fact]
        public void TestSetElementWhenElementIsSetToCorrenctIndex()
        {
            var sortList = new SortedList<int>();
            sortList.Add(3);
            sortList.Add(5);
            sortList.Add(7);
            sortList[1] = 4;
            Assert.Equal(4, sortList[1]);
        }

        [Fact]
        public void TestSetElementWhenElementIsNotSetToCorrenctIndex()
        {
            var sortList = new SortedList<int>();
            sortList.Add(2);
            sortList.Add(5);
            sortList.Add(3);
            sortList.Add(4);
            sortList[0] = 10;
            Assert.Equal(2, sortList[0]);
        }

        [Fact]
        public void Contains()
        {
            var sortList = new SortedList<int>();
            sortList.Add(1);
            sortList.Add(2);
            sortList.Add(3);
            sortList.Add(3);
            sortList.Add(5);
            sortList.Add(6);
            sortList[3] = 10;
            Assert.True(sortList.Contains(1));
            Assert.True(sortList.Contains(2));
            Assert.True(sortList.Contains(3));
            Assert.False(sortList.Contains(10));
        }

        [Fact]
        public void TestIndexOf()
        {
            var sortList = new SortedList<int>();
            sortList.Add(1);
            sortList.Add(2);
            sortList.Add(3);
            Assert.Equal(0, sortList.IndexOf(1));
            Assert.Equal(1, sortList.IndexOf(2));
            Assert.Equal(2, sortList.IndexOf(3));
            Assert.Equal(-1, sortList.IndexOf(4));
        }

        [Fact]
        public void TestInsert()
        {
            var sortList = new SortedList<int>();
            sortList.Add(3);
            sortList.Add(6);
            sortList.Add(9);
            sortList.Add(17);
            sortList.Add(15);
            sortList.Insert(1, 5);
            Assert.Equal(6, sortList.Count);
            Assert.True(sortList.Contains(5));
            Assert.Equal(1, sortList.IndexOf(5));
            sortList.Insert(0, 18);
            Assert.Equal(6, sortList.Count);
            Assert.False(sortList.Contains(18));
        }

        [Fact]
        public void TestClear()
        {
            var sortList = new SortedList<int>();
            sortList.Add(1);
            sortList.Add(2);
            sortList.Add(3);
            sortList.Clear();
            Assert.Equal(0, sortList.Count);
            Assert.False(sortList.Contains(1));
        }

        [Fact]
        public void TestRemove()
        {
            var sortList = new SortedList<int>();
            sortList.Add(1);
            sortList.Add(6);
            sortList.Add(3);
            sortList.Add(5);
            sortList.Add(2);
            sortList.Remove(3);
            Assert.Equal(4, sortList.Count);
            Assert.False(sortList.Contains(3));
            sortList.Remove(7);
            Assert.Equal(4, sortList.Count);
            Assert.False(sortList.Contains(7));
            sortList[2] = 4;
            sortList.Remove(4);
            Assert.Equal(3, sortList.Count);
            Assert.False(sortList.Contains(4));
            Assert.Equal(1, sortList[0]);
            Assert.Equal(2, sortList[1]);
            Assert.Equal(6, sortList[2]);
        }

        [Fact]
        public void TestRemoveAt()
        {
            var sortList = new SortedList<int>();
            sortList.Add(7);
            sortList.Add(3);
            sortList.Add(5);
            sortList.Add(7);
            sortList.Add(6);
            sortList.Add(12);
            sortList.RemoveAt(3);
            sortList.Add(10);
            sortList.Add(10);
            sortList[0] = 2;
            sortList[1] = 8;
            Assert.Equal(2, sortList[0]);
            Assert.False(sortList.Contains(8));
            sortList.RemoveAt(0);
            Assert.Equal(6, sortList.Count);
            Assert.Equal(2, sortList.IndexOf(7));
            Assert.Equal(3, sortList.IndexOf(10));
        }
    }
}
