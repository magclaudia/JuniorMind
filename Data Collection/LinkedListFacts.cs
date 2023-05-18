using System;
using Xunit;

namespace DataCollection
{
    public class LinkedListFacts
    {
        [Fact]
        public void TestAddFirstLinkedList() 
        {
            var list = new LinkedList<string>();
            var node = new LinkedListNode<string>("one");
            list.AddFirst(node);
            list.AddFirst("two");
            list.AddFirst("five");
            list.AddFirst("six");
            Assert.Equal(4, list.Count);
            Assert.Equal("one", node.Value);
            Assert.Equal("two", node.Previous.Value);
            Assert.Equal("five", node.Next.Next.Value);
            Assert.Equal("six", node.Next.Next.Value);
        }
    }
}
