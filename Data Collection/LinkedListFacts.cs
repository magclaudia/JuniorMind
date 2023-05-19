using System;
using Xunit;

namespace DataCollection
{
    public class LinkedListFacts
    {
        [Fact]
        public void TestAddAfter()
        {
            var linkedListNode = new LinkedListNode<int>(2);
            var list = new LinkedList<int>();
            var linkedListNodeToAdd = new LinkedListNode<int>(3);
            list.AddLast(linkedListNode);
            list.AddFirst(1);
            list.AddLast(4);
            list.AddLast(5);
            list.AddAfter(linkedListNode, linkedListNodeToAdd);
            Assert.Equal(5, list.Count);
            Assert.Equal(2, linkedListNodeToAdd.Previous.Value);
            Assert.Equal(4, linkedListNodeToAdd.Next.Value);
        }

        [Fact]
        public void TestAddFirst()
        {
            var list = new LinkedList<int>();
            var node = new LinkedListNode<int>(1);
            list.AddFirst(node);
            list.AddFirst(3);
            list.AddFirst(6);
            Assert.Equal(3, list.Count);
            Assert.Equal(1, node.Value);
            Assert.Equal(6, node.Next.Next.Value);
            Assert.Equal(3, node.Previous.Value);
        }

        [Fact]
        public void TestAddBefore()
        {
            var linkedListNode = new LinkedListNode<int>(4);
            var list = new LinkedList<int>();
            var linkedListNodeToAdd = new LinkedListNode<int>(3);
            list.AddLast(linkedListNode);
            list.AddFirst(2);
            list.AddFirst(1);
            list.AddFirst(5);
            list.AddBefore(linkedListNode, linkedListNodeToAdd);
            Assert.Equal(5, list.Count);
            Assert.Equal(2, linkedListNodeToAdd.Previous.Value);
            Assert.Equal(4, linkedListNodeToAdd.Next.Value);
        }

        [Fact]
        public void TestAddLast()
        {
            var list = new LinkedList<int>();
            var node = new LinkedListNode<int>(1);
            list.AddLast(node);
            list.AddLast(3);
            list.AddLast(6);
            Assert.Equal(3, list.Count);
            Assert.Equal(1, node.Value);
            Assert.Equal(3, node.Next.Value);
            Assert.Equal(6, node.Previous.Previous.Value);
        }

        [Fact] 
        public void TestFind() 
        {
            var list = new LinkedList<int>();
            var node = new LinkedListNode<int>(1);
            list.AddLast(node);
            list.AddLast(3);
            list.AddLast(6);
            list.AddLast(4);
            Assert.Equal(4, list.Count);
            Assert.Equal(1, node.Value);
            Assert.Equal(node, list.Find(1));
            Assert.Null(list.Find(10));
        }

        [Fact] 
        public void TestContains() 
        {
            var list = new LinkedList<int>() { 1, 2, 3};
            Assert.True(list.Contains(1));
            Assert.False(list.Contains(4));
        }

        [Fact]
        public void TestFindLast()
        {
            var list = new LinkedList<int>();
            var node = new LinkedListNode<int>(1);
            list.AddLast(node);
            list.AddLast(3);
            list.AddLast(6);
            list.AddLast(4);
            Assert.Equal(4, list.Count);
            Assert.Equal(1, node.Value);
            Assert.Equal(node, list.FindLast(1));
            Assert.Null(list.FindLast(10));
        }

        [Fact]
        public void TestClear()
        {
            var list = new LinkedList<int>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);
            list.Clear();
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void TestRemove()
        {
            var list = new LinkedList<int>();
            var node = new LinkedListNode<int>(1);
            list.AddLast(node);
            list.AddLast(3);
            list.AddLast(5);
            list.Remove(node);
            Assert.Equal(2, list.Count);
            Assert.Equal(list.Find(3).Next, list.FindLast(5));
        }

        [Fact]
        public void TestRemoveFirst() 
        {
            var list = new LinkedList<int>();
            var node = new LinkedListNode<int>(1);
            list.AddLast(3);
            list.AddLast(5);
            list.AddFirst(node);
            list.RemoveFirst();
            Assert.Equal(2, list.Count);
            Assert.Equal(list.FindLast(5), node.Previous.Previous);
        }

        [Fact]
        public void TestRemoveLast() 
        {
            var list = new LinkedList<int>();
            var node = new LinkedListNode<int>(1);
            list.AddLast(3);
            list.AddLast(5);
            list.AddFirst(node);
            list.RemoveLast();
            Assert.Equal(2, list.Count);
            Assert.Equal(list.FindLast(3), node.Previous.Previous);
        }

        [Fact]
        public void TestCopyTo()
        {
            var list = new LinkedList<int>() { 1, 2, 3};
            int[] array = new int[5];
            list.CopyTo(array, 0); 
            string result = array[0].ToString() + array[1].ToString() + array[2].ToString() + array[3].ToString() + array[4].ToString();
            Assert.Equal(5, array.Length);
            Assert.Equal("12300", result);
        }
    }
}
