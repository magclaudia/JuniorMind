using System;
using System.Collections.Generic;
using Xunit;

namespace DataCollection
{
    public class ListFacts
    {
        [Fact]
        public void TestAddAndCount()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(6.501);
            list.Add('/');
            list.Add(7);
            list.Add(6);
            list.Add('s');
            list.Add(7);
            Assert.Equal(9, list.Count);
        }

        [Fact]
        public void TestElement()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(6.501);
            list.Add('/');
            list.Add(7);
            list.Add('s');
            Assert.Equal(1, list[0]);
            Assert.Equal("sgsdhgjd", list[1]);
            Assert.Equal(-5, list[2]);
            Assert.Equal(6.501, list[3]);
            Assert.Equal('/', list[4]);
            Assert.Equal(7, list[5]);
            Assert.Equal('s', list[6]);
        }

        [Fact]
        public void TestSetElement()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(6.501);
            list.Add('/');
            list[1] = 4;
            Assert.Equal(4, list[1]);
        }

        [Fact]
        public void Contains()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(6.501);
            list.Add('/');
            Assert.True(list.Contains(1));
            Assert.True(list.Contains("sgsdhgjd"));
            Assert.True(list.Contains(-5));
            Assert.True(list.Contains(6.501));
            Assert.True(list.Contains('/'));
        }

        [Fact]
        public void TestIndexOf()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(6.501);
            list.Add('/');
            Assert.Equal(0, list.IndexOf(1));
            Assert.Equal(1, list.IndexOf("sgsdhgjd"));
            Assert.Equal(2, list.IndexOf(-5));
            Assert.Equal(3, list.IndexOf(6.501));
            Assert.Equal(4, list.IndexOf('/'));
            Assert.Equal(-1, list.IndexOf(4));
        }

        [Fact]
        public void TestInsert()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(6.501);
            list.Add('/');
            list.Insert(1, 10);
            Assert.Equal(6, list.Count);
            Assert.Equal(1, list[0]);
            Assert.Equal(10, list[1]);
            Assert.Equal("sgsdhgjd", list[2]);
            Assert.Equal(-5, list[3]);
            Assert.Equal(6.501, list[4]);
            Assert.Equal('/', list[5]);
        }

        [Fact]
        public void TestClear()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(6.501);
            list.Add('/');
            list.Clear();
            Assert.Null(list[0]);
        }

        [Fact]
        public void TestRemove()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(3);
            list.Add(6.501);
            list.Add('/');
            list.Remove(3);
            Assert.Equal(5, list.Count);
            list.Remove(6);
            Assert.Equal(5, list.Count);
            Assert.Equal(1, list[0]);
            Assert.Equal("sgsdhgjd", list[1]);
            Assert.Equal(-5, list[2]);
            Assert.Equal(6.501, list[3]);
            Assert.Equal('/', list[4]);
        }

        [Fact]
        public void TestRemoveAt()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(3);
            list.Add(6.501);
            list.Add('/');
            list.RemoveAt(3);
            Assert.Equal(5, list.Count);
            Assert.Equal(1, list[0]);
            Assert.Equal("sgsdhgjd", list[1]);
            Assert.Equal(-5, list[2]);
            Assert.Equal(6.501, list[3]);
            Assert.Equal('/', list[4]);
        }

        [Fact]
        public void TestCopyTo()
        {
            var list = new List<object>();
            object[] array = { 0, 2, 8, 6, "5", '8', 4, 7 };
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(3);
            list.CopyTo(array, 1);
            Assert.Equal(new object[] { 0, 1, "sgsdhgjd", -5, 3, '8', 4, 7 }, array);
            object[] newArray1 = { 0, 2, 8, 6, "5", '8', 4, 7 };
            list.CopyTo(newArray1, 3);
            Assert.Equal(new object[] { 0, 2, 8, 1, "sgsdhgjd", -5, 3, 7 }, newArray1);
        }

        [Fact]
        public void TestReadOnly()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(3);
            ReadOnlyList<int> readonlyList = list.ToReadOnly();
            Assert.Throws<NotSupportedException>(() => readonlyList.Add(4));
        }

        [Fact]
        public void TestIndexArgumentOutOfRangeException()
        {
            var list = new List<object>();
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(6.501);
            list.Add('/');
            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[6]);
        }

        [Fact]
        public void TestCopyToForArgumentNullExceptiont()
        {
            var list = new List<object>();
            object[] array = null;
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(3);
            Assert.Throws<ArgumentNullException>(() => list.CopyTo(array, 1));
        }

        [Fact]
        public void TestCopyToForArgumentOutOfRangeException()
        {
            var list1 = new List<object>();
            object[] array = { 0, 2, 8, 6, "5", '8', 4, 7 };
            list1.Add(1);
            list1.Add("sgsdhgjd");
            list1.Add(-5);
            list1.Add(3);
            Assert.Throws<ArgumentOutOfRangeException>(() => list1.CopyTo(array, -2));
            var list2 = new List<object>();
            object[] array1 = { 0, 2, 8 };
            list2.Add(7);
            Assert.Throws<ArgumentOutOfRangeException>(() => list2.CopyTo(array1, 4));
        }

        [Fact]
        public void TestCopyToForArgumentException()
        {
            var list = new List<object>();
            object[] array = { 0, 8, 6, "5", 7 };
            list.Add(1);
            list.Add("sgsdhgjd");
            list.Add(-5);
            list.Add(3);
            Assert.Throws<ArgumentException>(() => list.CopyTo(array, 4));
        }
    }
}
