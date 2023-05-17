using System;
using System.Collections.Generic;

namespace DataCollection
{
    public sealed class LinkedListNode<T>
    {
        public LinkedList<T> List { get; set; }
        public LinkedListNode<T> Next { get; set; }
        public LinkedListNode<T> Previous { get; set; }
        public T Value { get; set; }
    }
}
